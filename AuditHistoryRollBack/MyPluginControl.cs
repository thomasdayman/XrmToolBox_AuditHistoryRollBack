using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.ServiceModel;
using System.Threading;


// ============================================================================
// ============================================================================
// ============================================================================
namespace AuditHistoryRollBack
{


    // ============================================================================
    // ============================================================================
    // ============================================================================
    public partial class MyPluginControl : PluginControlBase
    {
        internal Settings mySettings = null;
        private List<AuditRecord> AuditRecords = new List<AuditRecord>();
        private List<string> newestAuditRecords = new List<string>();
        private List<int> oldAuditRecords = new List<int>();
        private BindingSource bindingSource;
        private Entity TargetEntity;


        private string[] ValidActions =
            { "Update",
              "Activate",
              "Deactivate",
              "Assign"
            };


        // ============================================================================
        public MyPluginControl()
        {
            InitializeComponent();
        }

        // ============================================================================
        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            if (!AuthTypeCheck()) return;

            LoadSettings();
            LoadEntities();
        }


        // ============================================================================
        internal bool AuthTypeCheck()
        {
            if (ConnectionDetail?.AuthType != null &&
                ConnectionDetail?.NewAuthType != null &&
                ConnectionDetail.AuthType != Microsoft.Xrm.Sdk.Client.AuthenticationProviderType.OnlineFederation &&
                (
                    ConnectionDetail.NewAuthType != Microsoft.Xrm.Tooling.Connector.AuthenticationType.AD ||
                    ConnectionDetail.NewAuthType != Microsoft.Xrm.Tooling.Connector.AuthenticationType.OAuth ||
                    ConnectionDetail.NewAuthType != Microsoft.Xrm.Tooling.Connector.AuthenticationType.ClientSecret
                ))
            {
                MessageBox.Show("Your connection type is not supported, Please connect using SDK Login Control to use this Tool.");
                return false;
            }
            return true;
        }

        // ============================================================================
        /// <summary>Loads configurations from file</summary>
        private void LoadSettings()
        {
            mySettings = null;

            LogInfo("Loading settings now...");

            try
            {
                SettingsManager.Instance.TryLoad(
                    typeof(MyPluginControl),
                    out mySettings);

                if (mySettings != null)
                {
                    showNewestValues.Checked =
                        mySettings.ShowNewestAuditsOnly;

                    autoCopyGuidFromClipboard.Checked =
                        mySettings.AutoCopyGuidFromClipboard;
                }

            }
            catch (InvalidOperationException)
            {
                LogWarning("Settings could not be loaded!");
            }

            if (mySettings == null)
            {
                mySettings = new Settings();
                SaveSettings();
                LogWarning("A new settings-file was created");
            }
            else
            {
                LogWarning("Settings Found: " + mySettings);
            }

        }


        // ============================================================================
        private void SaveSettings()
        {
            if (mySettings != null)
            {
                mySettings.AutoCopyGuidFromClipboard =
                    autoCopyGuidFromClipboard.Checked;

                if (entitiesList.SelectedIndex != -1)
                {
                    mySettings.LastUsedEntity =
                        entitiesList.SelectedItem.ToString();
                }

                SettingsManager.Instance.Save(
                    typeof(MyPluginControl),
                    mySettings);

                LogInfo("Settings-file was saved.");
                {
                    mySettings.AutoCopyGuidFromClipboard =
                   autoCopyGuidFromClipboard.Checked;

                    if (entitiesList.SelectedIndex != -1)
                    {
                        mySettings.LastUsedEntity =
                            entitiesList.SelectedItem.ToString();
                    }

                    SettingsManager.Instance.Save(
                        typeof(MyPluginControl),
                        mySettings);
                }
            }
        }


        // ============================================================================
        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }


        // ============================================================================
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            LogInfo("The plugin {0} is shutting down.", ToString());
            SaveSettings();
        }

        // ============================================================================
        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        // ============================================================================
        private void WhoAmI()
        {
            Service.Execute(new WhoAmIRequest());
        }


        // ============================================================================
        public void LoadEntities()
        {
            LogInfo("Loading entites now...");

            if (entitiesList != null)
            {
                entitiesList.Items.Clear();
            }

            disableButtons();

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Entities...",
                Work = (bw, args) =>
                {
                    try
                    {
                        RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest
                        {
                            EntityFilters = EntityFilters.Entity,
                            RetrieveAsIfPublished = true

                        };

                        args.Result = (RetrieveAllEntitiesResponse)Service.Execute(request);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + " | " + ex?.InnerException?.Message);
                    }

                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(
                            args.Error.ToString(),
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }

                    var response = (RetrieveAllEntitiesResponse)args.Result;
                    var counter = 0;

                    foreach (EntityMetadata entity in response.EntityMetadata)
                    {
                        if (entity.IsAuditEnabled.Value == true)
                        {
                            entitiesList.Items.Add(entity.LogicalName);
                            counter++;
                        }
                    }

                    LogInfo("Added {0} entities with enabled audit to the list.", counter);

                    entitiesList.Sorted = true;
                    enableButtons();

                    // apply settings of last used entity
                    var index =
                    entitiesList.FindString(
                        mySettings.LastUsedEntity);

                    LogInfo("Found last-used-entity at index: " + mySettings.LastUsedEntity + " -> " + index);

                    if (index != -1)
                    {
                        entitiesList.SelectedIndex = index;
                    }
                }
            });
        }


        // ============================================================================
        private void LoadAuditHistoryButton(object sender, EventArgs e)
        {
            LoadAuditHistory();
        }


        // ============================================================================
        private void LoadAuditHistory()
        {
            if (entitiesList.SelectedItem != null)
            {
                ClearAuditHistory();
                disableButtons();

                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Loading Audit History...",
                    Work = (bw, d) => {
                        LoadAuditHistoryLogic();
                        //HideOldAuditRecords();
                    },
                    PostWorkCallBack = (d) =>
                    {
                        HideOldAuditRecords();
                        ShowAuditHistory();
                        enableButtons();
                    }
                });
            }
            else
            {
                MessageBox.Show("You have not selected an entity");
            }
        }


        // ============================================================================
        private void LoadAuditHistoryLogic()
        {
            if (Service != null)
            {
                RetrieveRecordChangeHistoryRequest changeRequest = new RetrieveRecordChangeHistoryRequest();
                try
                {
                    Guid guid;
                    if (!Guid.TryParse(recordGuid.Text, out guid))
                    {
                        return;
                    }

                    changeRequest.Target = new EntityReference(entitiesList.SelectedItem.ToString(), guid);
                    RetrieveRecordChangeHistoryResponse changeResponse =
                    (RetrieveRecordChangeHistoryResponse)Service.Execute(changeRequest);

                    AuditDetailCollection auditDetailCollection = changeResponse.AuditDetailCollection;

                    LogInfo("Retrieved {0} audit entires for this record.", auditDetailCollection.Count);

                    Entity entity = Service.Retrieve(entitiesList.SelectedItem.ToString(), guid, new ColumnSet(true));
                    TargetEntity = new Entity(entity.LogicalName, entity.Id);

                    foreach (var attrAuditDetail in auditDetailCollection.AuditDetails)
                    {
                        var detailType = attrAuditDetail.GetType();
                        if (detailType == typeof(AttributeAuditDetail))
                        {
                            var auditRecord = attrAuditDetail.AuditRecord;
                            var attributeDetail = (AttributeAuditDetail)attrAuditDetail;

                            var newValueEntity = attributeDetail.NewValue;
                            var oldValueEntity = attributeDetail.OldValue;

                            var action = auditRecord.FormattedValues["action"];
                            if (Array.Exists(ValidActions, x => x == action))
                            {

                                foreach (KeyValuePair<string, object> attribute in attributeDetail.NewValue.Attributes)
                                {
                                    string fieldmetadata = "";
                                    string lookupId = "";
                                    string oldValue = "";
                                    string newValue = "";

                                    if (attributeDetail.OldValue.Contains(attribute.Key))
                                    {
                                        lookupId = GetLookupId(attribute.Key, attribute.Value, oldValueEntity, attribute.Key);
                                        fieldmetadata = GetFieldMetaData(attribute.Key, attribute.Value, oldValueEntity, attribute.Key);
                                        oldValue = GetFieldValue(attribute.Key, attribute.Value, oldValueEntity, oldValueEntity[attribute.Key].ToString());
                                    }
                                    newValue = GetFieldValue(attribute.Key, attribute.Value, newValueEntity, newValueEntity[attribute.Key].ToString());

                                    CreateAuditRecord(auditRecord.Attributes["auditid"], auditRecord.Attributes["createdon"], ((EntityReference)auditRecord.Attributes["userid"]).Name,
                                        auditRecord.FormattedValues["action"], attribute.Key, oldValue, newValue, attribute.Value, fieldmetadata, lookupId);
                                }

                                foreach (KeyValuePair<String, object> attribute in attributeDetail.OldValue.Attributes)
                                {
                                    if (!attributeDetail.NewValue.Contains(attribute.Key))
                                    {
                                        string fieldmetadata = "";
                                        string loookupId = "";
                                        string newValue = "";

                                        loookupId = GetLookupId(attribute.Key, attribute.Value, oldValueEntity, attribute.Key);
                                        fieldmetadata = GetFieldMetaData(attribute.Key, attribute.Value, oldValueEntity, attribute.Key);
                                        string oldValue = GetFieldValue(attribute.Key, attribute.Value, oldValueEntity, oldValueEntity[attribute.Key].ToString());

                                        CreateAuditRecord(auditRecord.Attributes["auditid"], auditRecord.Attributes["createdon"], ((EntityReference)auditRecord.Attributes["userid"]).Name,
                                          auditRecord.FormattedValues["action"], attribute.Key, oldValue, newValue, attribute.Value, fieldmetadata, loookupId);

                                    }
                                }
                            }
                        }
                    }
                }
                catch (InvalidPluginExecutionException ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
                catch (TimeoutException ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }


        private void ShowAuditHistory()
        {
            var bindingList = new BindingList<AuditRecord>(AuditRecords);
            bindingSource = new BindingSource(bindingList, null);
            dataGridView1.DataSource = bindingSource;

            dataGridView1.AutoResizeColumns();
            dataGridView1.ClearSelection();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;

            if (dataGridView1.RowCount > 0)
            {
                showNewestValues.Enabled = true;
            }
            else
            {
                showNewestValues.Enabled = false;
            }
        }

        private void ClearAuditHistory()
        {
            dataGridView1.Rows.Clear();
            AuditRecords.Clear();
        }

        private string GetFieldValue(string attribute, object type, Entity entity, string value)
        {

            if (type is OptionSetValue)
            {
                return entity.FormattedValues[attribute];
            }
            else if (type is EntityReference)
            {
                return entity.GetAttributeValue<EntityReference>(attribute.ToString()).Name;
            }
            else if (type is Money)
            {
                return entity.GetAttributeValue<Money>(attribute.ToString()).Value.ToString();
            }
            else
            {
                return value;
            }
        }

        private string GetFieldMetaData(string attribute, object type, Entity entity, string attributeValue)
        {
            if (type is EntityReference)
            {
                EntityReference entityRef = (EntityReference)entity.Attributes[attribute];

                string EntityType = entityRef.LogicalName;
                return EntityType;
            }
            if (type is OptionSetValue)
            {
                if (attributeValue != null)
                {
                    string EntityType = entity.GetAttributeValue<OptionSetValue>(attribute).Value.ToString();
                    return EntityType;
                }
                return "";
            }
            else
            {
                return "";
            }
        }

        private string GetLookupId(string attribute, object type, Entity entity, string attributeValue)
        {
            if (type is EntityReference)
            {
                EntityReference entityRef = (EntityReference)entity.Attributes[attribute];

                string LookupId = entityRef.Id.ToString();
                return LookupId;
            }
            else
            {
                return "";
            }
        }

        private void CreateAuditRecord(object auditId, object createdon, string userid, string action, string field, string oldvalue, string newvalue, object type, string entitylookupname, string lookupid)
        {
            AuditRecords.Add(new AuditRecord
            {
                AuditId = auditId,
                CreatedOn = createdon,
                UserId = userid,
                Action = action,
                Field = field,
                OldValue = oldvalue,
                NewValue = newvalue,
                Type = type,
                FieldMetaData = entitylookupname,
                LookupId = lookupid,
            });
        }

        private void RollBackAudit(DataGridView dataGrid, Entity changeRequest)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Rollingback...",
                Work = (bw, e) => {
                    try
                    {
                        List<DataGridViewRow> rows =
                            (from DataGridViewRow row in dataGridView1.SelectedRows
                             where !row.IsNewRow
                             orderby row.Index
                             select row).ToList<DataGridViewRow>();

                        foreach (DataGridViewRow auditinfo in rows)
                        {
                            var type = auditinfo.Cells[7].Value;
                            var field = auditinfo.Cells[4].Value.ToString();
                            var newValue = auditinfo.Cells[6].Value.ToString();
                            var oldValue = auditinfo.Cells[5].Value.ToString();
                            var fieldmetadata = auditinfo.Cells[8].Value.ToString();
                            var lookupId = auditinfo.Cells[9].Value.ToString();
                            UpdateAudit(Service, changeRequest, type, field, newValue, oldValue, fieldmetadata, lookupId);
                            //Service.Update(changeRequest);
                        }
                        //Service.Update(changeRequest);
                    }
                    catch (InvalidPluginExecutionException ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                    catch (FaultException<OrganizationServiceFault> ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                    catch (TimeoutException ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                },
                PostWorkCallBack = (e) =>
                {
                    Thread.Sleep(500);
                    LoadAuditHistory();
                }
            });
        }

        private void UpdateAudit(IOrganizationService service, Entity entity, object type, string field, string newvalue, string oldvalue, string fieldmetadata, string lookupId)
        {
            if (!string.IsNullOrEmpty(oldvalue as string))
            {
                switch (type)
                {
                    case OptionSetValue _:
                        entity.Attributes[field] = new OptionSetValue(Convert.ToInt32(fieldmetadata));
                        break;
                    case EntityReference _:
                        entity.Attributes[field] = new EntityReference(fieldmetadata, new Guid(lookupId));
                        break;
                    case Money _:
                        entity.Attributes[field] = new Money(Convert.ToDecimal(oldvalue));
                        break;
                    case bool _:
                        entity.Attributes[field] = Convert.ToBoolean(oldvalue);
                        break;
                    case int _:
                        entity.Attributes[field] = Convert.ToInt32(oldvalue);
                        break;
                    case DateTime _:
                        entity.Attributes[field] = Convert.ToDateTime(oldvalue);
                        break;
                    case decimal _:
                        entity.Attributes[field] = Convert.ToDecimal(oldvalue);
                        break;
                    case float _:
                        entity.Attributes[field] = float.Parse(oldvalue);
                        break;
                    case double _:
                        entity.Attributes[field] = Convert.ToDouble(oldvalue);
                        break;
                    default:
                        entity.Attributes[field] = oldvalue;
                        break;
                }
            }
            else
            {
                entity.Attributes[field] = null;
            }
            Service.Update(entity);
        }


        private void HideOldAuditRecords()
        {
            if (!Guid.TryParse(recordGuid.Text, out _))
            {
                return;
            }

            disableButtons();
            dataGridView1.ClearSelection();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            if (showNewestValues.Checked)
            {
                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Showing latest Audit Records...",
                    Work = (bw, k) =>
                    {
                        Thread.Sleep(500);
                        int index = 0;
                        foreach (DataGridViewRow auditinfo in dataGridView1.Rows)
                        {
                            var field = auditinfo.Cells[4].Value.ToString();
                            if (newestAuditRecords.Contains(field))
                            {
                                oldAuditRecords.Add(index);
                            }
                            else
                            {
                                newestAuditRecords.Add(field);
                            }
                            index++;
                        }
                        newestAuditRecords.Clear();
                    },
                    PostWorkCallBack = (k) =>
                    {
                        CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView1.DataSource];
                        currencyManager1.SuspendBinding();
                        foreach (var oldRecords in oldAuditRecords)
                        {
                            dataGridView1.Rows[oldRecords].Visible = false;
                        }
                        currencyManager1.ResumeBinding();
                        oldAuditRecords.Clear();
                    }
                });
            }
            else if (showNewestValues.Checked == false)
            {
                WorkAsync(new WorkAsyncInfo
                {
                    Message = "Showing all Audit Records...",
                    Work = (bw, k) =>
                    {
                        Thread.Sleep(500);
                    },
                    PostWorkCallBack = (k) =>
                    {
                        int index = 0;
                        foreach (DataGridViewRow auditinfo in dataGridView1.Rows)
                        {
                            dataGridView1.Rows[index].Visible = true;
                            index++;
                        }
                    }
                });
            }
            enableButtons();
        }

        private void rollbackbutton_Click(object sender, EventArgs e)
        {
            if (entitiesList.SelectedItem != null)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    RollBackAudit(dataGridView1, TargetEntity);
                }
                else
                {
                    MessageBox.Show("You have not selected any rows to rollback");
                }
            }
            else
            {
                MessageBox.Show("You have not selected an entity");
            }
        }

        private void LoadDataButton_Click(object sender, EventArgs e)
        {
            LoadEntities();
        }

        private void showNewestValues_CheckedChanged(object sender, EventArgs e)
        {
            mySettings.ShowNewestAuditsOnly =
                showNewestValues.Checked;

            SaveSettings();
            HideOldAuditRecords();
        }

        private void disableButtons()
        {
            LoadDataButton.Enabled = false;
            showNewestValues.Enabled = false;
            rollbackbutton.Enabled = false;
            loadAuditButton.Enabled = false;
        }
        private void enableButtons()
        {
            LoadDataButton.Enabled = true;
            showNewestValues.Enabled = true;
            rollbackbutton.Enabled = true;
            loadAuditButton.Enabled = true;
        }

        private void recordGuid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadAuditHistory();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                rollbackbutton.Enabled = false;
            }
            else
            {
                rollbackbutton.Enabled = true;
            }

        }

        private void recordGuid_Click(object sender, EventArgs e)
        {
            if (GetGuidFromClipboard())
            {
                LoadAuditHistory();
            }
        }


        private bool GetGuidFromClipboard()
        {
            if (autoCopyGuidFromClipboard.Checked)
            {
                var clipboard = Clipboard.GetText();

                if (Guid.TryParse(clipboard, out _))
                {
                    recordGuid.Text = clipboard;
                    return true;
                }
            }

            return false;
        }

        private void entitiesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void autoCopyGuidFromClipboard_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }

    // ============================================================================
    // ============================================================================
    // ============================================================================
    public class AuditRecord
    {
        public Object AuditId { get; set; }
        public Object CreatedOn { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Field { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public object Type { get; set; }
        public string FieldMetaData { get; set; }
        public string LookupId { get; set; }

    }


}
