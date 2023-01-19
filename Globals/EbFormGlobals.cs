using CodingSeb.ExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ExpressBase.CoreBase.Globals
{
    public class FG_Root
    {
        public dynamic form { get; private set; }

        public dynamic sourceform { get; private set; }

        public dynamic parameters { get; set; }

        public FG_User user { get; private set; }

        public FG_System system { get; private set; }

        public FG_DataDB datadb { get; private set; }

        public FG_Locations locations { get; set; }

        public dynamic destinationform { get; set; }

        public List<FG_WebForm> DestinationForms { get; set; }

        public Dictionary<int, object[]> out_dict { get; set; }

        public FG_Root(FG_WebForm fG_WebForm)
        {
            this.form = fG_WebForm;
        }

        public FG_Root(FG_WebForm fG_WebForm, FG_User fG_User)
        {
            this.form = fG_WebForm;
            this.user = fG_User;
        }

        public FG_Root(FG_WebForm fG_WebForm, FG_User fG_User, FG_System fG_System, bool isSrcForm, FG_DataDB fG_DataDB, FG_Locations fG_Locations)
        {
            if (isSrcForm)
                this.sourceform = fG_WebForm;
            else
                this.form = fG_WebForm;
            this.user = fG_User;
            this.system = fG_System;
            this.datadb = fG_DataDB;
            this.locations = fG_Locations;
        }

        public FG_Root(FG_Params fG_Params)
        {
            this.parameters = fG_Params;
        }

        #region ScriptFunctions

        public void InitOutputDict()
        {
            out_dict = new Dictionary<int, object[]>();
        }

        public void AddToOutDict(int key, object value, object curItem)
        {
            if (curItem is FG_Control fgCtrl)
                fgCtrl.setValue(value);

            out_dict.Add(key, new object[] { 1, value });
        }

        public void AddToOutDict(int key, Exception e)
        {
            out_dict.Add(key, new object[] { 2, e.Message });
        }

        #endregion
    }

    public class FG_User
    {
        public int UserId { get; private set; }

        public string FullName { get; private set; }

        public string Email { get; private set; }

        public List<string> Roles { get; private set; }

        public FG_User(int userId, string fullName, string email, List<string> roles)
        {
            this.UserId = userId;
            this.FullName = fullName;
            this.Email = email;
            this.Roles = roles;
        }
    }

    public delegate object ExecuteScalarDelegate(string Qry);

    public class FG_DataDB
    {
        public ExecuteScalarDelegate ScalarDel { get; private set; }

        public FG_DataDB(ExecuteScalarDelegate ScalarDel)
        {
            this.ScalarDel = ScalarDel;
        }

        public object executeScalar(string query)
        {
            return this.ScalarDel(query);
        }
    }

    public class FG_System
    {
        public List<FG_Notification> Notifications { get; set; }
        public List<FG_EmailNotification> EmailNotifications { get; set; }
        public List<FG_Notification> PushNotifications { get; set; }

        public FG_System()
        {
            this.Notifications = new List<FG_Notification>();
            this.EmailNotifications = new List<FG_EmailNotification>();
            this.PushNotifications = new List<FG_Notification>();
        }

        public void sendNotificationByUserId(int userId, string title = null)
        {
            this.Notifications.Add(new FG_Notification { UserId = userId, Title = title, NotifyBy = FG_NotifyBy.UserId });
        }

        public void sendNotificationByRoleIds(List<int> roleIds, string title = null)
        {
            this.Notifications.Add(new FG_Notification { RoleIds = roleIds, Title = title, NotifyBy = FG_NotifyBy.RoleIds });
        }

        public void sendNotificationByUserGroupIds(List<int> ugIds, string title = null)
        {
            this.Notifications.Add(new FG_Notification { UserGroupIds = ugIds, Title = title, NotifyBy = FG_NotifyBy.UserGroupIds });
        }

        public void sendEmail(string refId)
        {
            if (!string.IsNullOrWhiteSpace(refId))
                this.EmailNotifications.Add(new FG_EmailNotification { RefId = refId });
        }

        public void sendPushNotificationByUserId(int userId, string title, string message)
        {
            this.PushNotifications.Add(new FG_Notification { UserId = userId, Title = title, Message = message, NotifyBy = FG_NotifyBy.UserId });
        }
    }

    public class FG_Notification
    {
        public int UserId { get; set; }

        public List<int> RoleIds { get; set; }

        public List<int> UserGroupIds { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public FG_NotifyBy NotifyBy { get; set; }
    }

    public class FG_EmailNotification
    {
        public string RefId { get; set; }
    }

    public class FG_Locations : List<FG_Location>
    {
        public FG_Locations() : base() { }

        public FG_Location getLocationById(int locId)
        {
            return this.Find(e => e.LocId == locId);
        }

        public FG_Location getLocationByShortName(string sname)
        {
            return this.Find(e => e.ShortName == sname);
        }
    }

    public class FG_Location
    {
        public int LocId { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public bool IsGroup { get; set; }

        public int ParentId { get; set; }

        public string LongName { get; set; }

        public string ShortName { get; set; }

        public string Logo { get; set; }

        public string WeekHoliday1 { get; set; }

        public string WeekHoliday2 { get; set; }

        public Dictionary<string, string> Meta { get; set; }
    }

    public enum FG_NotifyBy
    {
        UserId = 1,
        RoleIds = 2,
        UserGroupIds = 3
    }

    public class FG_WebForm : DynamicObject
    {
        public string MasterTable { get; private set; }

        public FG_Row FlatCtrls { get; set; }

        public List<FG_DataGrid> DataGrids { get; set; }

        public FG_Review Review { get; set; }

        public int eb_loc_id { get; private set; }

        public string eb_ref_id { get; private set; }

        public int eb_created_by { get; private set; }

        public string eb_created_at { get; private set; }

        public string __mode { get; private set; }// "new" "edit"

        public dynamic id { get; private set; }

        public FG_WebForm(string masterTbl, int dataId, int locId, string refId, int createdBy, string createdAt)
        {
            this.MasterTable = masterTbl;
            if (dataId > 0)
                this.id = dataId;
            else
                this.id = $"{FG_Constants.DataId_PlaceHolder}(SELECT eb_currval('{masterTbl}_id_seq'))";
            this.eb_loc_id = locId;
            this.eb_ref_id = refId;
            this.eb_created_by = createdBy;
            this.eb_created_at = createdAt;
            this.__mode = dataId > 0 ? "edit" : "new";

            this.FlatCtrls = new FG_Row();
            this.DataGrids = new List<FG_DataGrid>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            result = this.FlatCtrls[name];
            if (result == null)
            {
                result = this.DataGrids.Find(e => e.Name == name);
                if (result == null && this.Review != null && name == "review")
                    result = this.Review;
            }
            if (result == null)
                throw new NullReferenceException(name + " is not a control");
            return true;
        }

        public object TryGetMember(string name)
        {
            object result = this.FlatCtrls[name];
            if (result == null)
            {
                result = this.DataGrids.Find(e => e.Name == name);
                if (result == null && this.Review != null && name == "review")
                    result = this.Review;
            }
            if (result == null)
                throw new NullReferenceException(name + " is not a control");
            return result;
        }

        public void UpdateCurrentRowOfDG(string DgName)
        {
            FG_DataGrid DG = DataGrids.Find(e => e.Name == DgName);
            if (DG == null)
                throw new Exception("Exception in UpdateCurrentRowOfDG- DG not found in: " + DgName);

            FG_Row row = new FG_Row();
            foreach (FG_Control fgCtrl in DG.RowModel.Controls)
            {
                row.Controls.Add(new FG_Control(fgCtrl.Name, fgCtrl.Type, fgCtrl.Value, fgCtrl.Metas));
            }
            DG.Rows.Add(row);
            DG.currentRow = row;
        }
    }

    public class FG_DataGrid
    {
        public string Name { get; private set; }

        public List<FG_Row> Rows { get; private set; }

        public FG_Row RowModel { get; private set; }

        public FG_Row currentRow { get; set; }

        public FG_DataGrid(string name, List<FG_Row> rows, FG_Row rowModel)
        {
            this.Name = name;
            this.Rows = rows;
            this.RowModel = rowModel;
        }

        public void setCurRow(int index)
        {
            if (this.Rows.Count > index && index >= 0)
                this.currentRow = this.Rows[index];
            throw new IndexOutOfRangeException($"{this.Name} (DataGrid) has only {this.Rows.Count} rows. Requested index: {index}");
        }

        public FG_Row getRowByIndex(int index)
        {
            if (this.Rows.Count > index && index >= 0)
                return this.Rows[index];
            throw new IndexOutOfRangeException($"{this.Name} (DataGrid) has only {this.Rows.Count} rows. Requested index: {index}");
        }

        public int GetEnumerator()
        {
            if (this.Rows.Count > 0)
            {
                if (this.currentRow == null)
                    this.currentRow = this.Rows[0];
                else
                {
                    int index = this.Rows.FindIndex(e => e == this.currentRow);
                    if ((this.Rows.Count - 1) > index)
                    {
                        this.currentRow = this.Rows[index + 1];
                        return index + 1;
                    }
                    else
                        this.currentRow = this.Rows[0];
                }
            }
            else
            {
                this.currentRow = null;
                return -1;
            }
            return 0;
        }

        public Double Sum(string cname)
        {
            Double s = 0;
            foreach (FG_Row Row in this.Rows)
            {
                if (Row[cname] != null)
                {
                    double.TryParse(Convert.ToString(Row[cname].getValue()), out double e);
                    s += e;
                }
            }
            return s;
        }
        public Double Avg(string cname)
        {
            Double s = 0;
            foreach (FG_Row Row in this.Rows)
            {
                if (Row[cname] != null)
                {
                    double.TryParse(Convert.ToString(Row[cname].getValue()), out double e);
                    s += e;
                }
            }
            return this.Rows.Count > 0 ? s / this.Rows.Count : s;
        }
        public Double Min(string cname)
        {
            Double s = 0;
            if (this.Rows.Count > 0)
                double.TryParse(Convert.ToString(this.Rows[0][cname].getValue()), out s);
            foreach (FG_Row Row in this.Rows)
            {
                if (Row[cname] != null)
                {
                    double.TryParse(Convert.ToString(Row[cname].getValue()), out double e);
                    if (e < s)
                        s = e;
                }
            }
            return s;
        }
        public Double Max(string cname)
        {
            Double s = 0;
            if (this.Rows.Count > 0)
                double.TryParse(Convert.ToString(this.Rows[0][cname].getValue()), out s);
            foreach (FG_Row Row in this.Rows)
            {
                if (Row[cname] != null)
                {
                    double.TryParse(Convert.ToString(Row[cname].getValue()), out double e);
                    if (e > s)
                        s = e;
                }
            }
            return s;
        }
    }

    public class FG_Review
    {
        public Dictionary<string, FG_Review_Stage> stages { get; private set; }

        public FG_Review_Stage currentStage { get; private set; }

        public string _ReviewStatus { get; private set; }
        public FG_Review_Stage _Stage { get; private set; }
        public string _Action { get; private set; }
        public string _Comments { get; private set; }

        public FG_Review(Dictionary<string, FG_Review_Stage> stages, FG_Review_Stage currentStage)
        {
            this.stages = stages;
            this.currentStage = currentStage;
        }

        public void complete()
        {
            this._ReviewStatus = "Completed";
        }

        public void abandon()
        {
            this._ReviewStatus = "Abandoned";
        }

        public void setCurrentStageDataEditable()
        {

        }

        public void complete(FG_Review_Stage stage, string action, string comments)
        {
            this._ReviewStatus = "AutoCompleted";
            this._Stage = stage;
            this._Action = action;
            this._Comments = comments;
        }
    }

    public class FG_Review_Stage
    {
        public string name { get; private set; }

        public List<FG_Review_Action> actions { get; private set; }

        public FG_Review_Action currentAction { get; private set; }

        public FG_Review_Stage(string Name, List<FG_Review_Action> Actions, FG_Review_Action currentAction)
        {
            this.name = Name;
            this.actions = Actions;
            this.currentAction = currentAction;
        }
    }

    public class FG_Review_Action
    {
        public string name { get; private set; }

        public FG_Review_Action(string Name)
        {
            this.name = Name;
        }
    }

    public class FG_Row : DynamicObject
    {
        public int id { get; set; }

        public List<FG_Control> Controls { get; set; }

        public FG_Row()
        {
            this.Controls = new List<FG_Control>();
        }

        public FG_Control this[string name]
        {
            get
            {
                FG_Control ctrl = this.Controls.Find(e => e.Name.Equals(name));
                //if (ctrl == null)
                //    Console.WriteLine($"Null ref in form globals. Name = {name}, CtrlCount = {this.Controls.Count}, RowId = {this.id}");
                return ctrl;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.Controls.Find(e => e.Name.Equals(binder.Name));
            if (result == null)
                throw new Exception(binder.Name + " is not a control");
            return true;
        }

        public FG_Control TryGetMember(string name)
        {
            FG_Control result = this.Controls.Find(e => e.Name.Equals(name));
            if (result == null)
                throw new Exception(name + " is not a control");
            return result;
        }
    }

    public class FG_Control
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public object Value { get; private set; }

        public Dictionary<string, object> Metas { get; private set; }

        public FG_Control(string Name, string Type, object Value, Dictionary<string, object> Metas)
        {
            this.Name = Name;
            this.Type = Type;
            this.Value = Value;
            this.Metas = Metas;
        }

        public object getValue()
        {
            return this.Value;
        }

        public void setValue(object Value)
        {
            this.Value = Value;
        }

        public object getColumn(string ColumnName)
        {
            if (this.Metas?.ContainsKey(FG_Constants.Columns) == true && this.Metas[FG_Constants.Columns] != null &&
                this.Metas[FG_Constants.Columns] is Dictionary<string, List<object>> Columns && Columns.ContainsKey(ColumnName) && Columns[ColumnName].Count > 0)
            {
                return Columns[ColumnName][0];
            }
            Console.WriteLine("getColumn value not found: " + this.Name);
            return null;
        }

        public string replacePrefixWith(string prefix)
        {
            string st = Convert.ToString(this.Value);
            if (st.Contains(FG_Constants.AutoId_PlaceHolder))
            {
                st = prefix + FG_Constants.AutoId_SerialNo_PlaceHolder;
                return st;
            }
            else if (this.Metas.ContainsKey(FG_Constants.SerialLength))
            {
                int iSL = Convert.ToInt32(this.Metas[FG_Constants.SerialLength]);
                if (st.Length <= iSL)
                    throw new Exception($"Exception in replacePrefixWith. Name: {this.Name}; st: {st}; SerialLength: {iSL};");
                st = prefix + st.Substring(st.Length - iSL);
                return st;
            }
            throw new Exception($"Inappropriate use of replacePrefixWith. Name: {this.Name}; st: {st};");
        }
    }


    public class FG_Params : DynamicObject
    {
        private Dictionary<string, FG_NV_List> Values { get; set; }

        public FG_Params(Dictionary<string, FG_NV_List> values)
        {
            this.Values = values;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            if (this.Values.ContainsKey(name))
            {
                result = this.Values[name];
                return true;
            }
            else
            {
                throw new NullReferenceException(name + " is not a key in global parameter dictionary");
            }
        }

        public FG_NV_List TryGetMember(string name)
        {
            if (this.Values.ContainsKey(name))
                return this.Values[name];
            throw new NullReferenceException(name + " is not a key in global parameter dictionary");
        }
    }

    public class FG_NV_List : DynamicObject
    {
        public List<FG_NV> Values { get; private set; }

        public FG_NV_List()
        {
            this.Values = new List<FG_NV>();
        }

        public void Add(FG_NV fG_NV)
        {
            this.Values.Add(fG_NV);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;
            FG_NV entry = this.Values.Find(e => e.Name == name);
            if (entry != null)
            {
                result = entry.Value;
                return true;
            }
            else
            {
                throw new NullReferenceException(name + " is not a in global parameter list");
            }
        }

        public object TryGetMember(string name)
        {
            FG_NV entry = this.Values.Find(e => e.Name == name);
            if (entry != null)
                return entry.Value;
            throw new NullReferenceException(name + " is not a in global parameter list");
        }
    }

    public class FG_NV
    {
        public string Name { get; private set; }

        public object Value { get; private set; }

        public FG_NV(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }

    public static class FG_Constants
    {
        //Control level
        public const string DataId_PlaceHolder = "__DataId_PlaceHolder__";
        public const string AutoId_PlaceHolder = "__EbAutoId_PlaceHolder__";
        public const string AutoId_SerialNo_PlaceHolder = "__EbAutoId_SerialNo_PlaceHolder__";
        public const string AutoId_Full_PlaceHolder = "__EbAutoId_Full_PlaceHolder__";

        //Object Types
        //public const string AutoId = "AutoId";
        //public const string PowerSelect = "PowerSelect";
        //public const string PowerSelectColumn = "PowerSelectColumn";

        //Meta Keys
        public const string Columns = "Columns";
        public const string SerialLength = "SerialLength";
    }

    public static class FG_Evaluator
    {
        public static object ScriptEvaluate(string code, FG_Root globals)
        {
            ExpressionEvaluator evaluator = new ExpressionEvaluator
            {
                OptionScriptNeedSemicolonAtTheEndOfLastExpression = false,
                Context = globals
            };
            evaluator.EvaluateVariable += EvaluateVariable;
            object obj = evaluator.ScriptEvaluate(code);
            return obj;
        }

        private static void EvaluateVariable(object sender, VariableEvaluationEventArg e)
        {
            if (e.This is FG_WebForm form)
            {
                e.Value = form.TryGetMember(e.Name);
            }
            else if (e.This is FG_Row row)
            {
                e.Value = row.TryGetMember(e.Name);
            }
            else if (e.This is FG_Params p)
            {
                e.Value = p.TryGetMember(e.Name);
            }
            else if (e.This is FG_NV_List nv)
            {
                e.Value = nv.TryGetMember(e.Name);
            }
        }
    }

}
