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
        
        public FG_Root(FG_WebForm fG_WebForm)
        {
            this.form = fG_WebForm;
        }

        public FG_Root(FG_WebForm fG_WebForm, FG_User fG_User)
        {
            this.form = fG_WebForm;
            this.user = fG_User;
        }

        public FG_Root(FG_WebForm fG_WebForm, FG_User fG_User, FG_System fG_System, int mode)
        {
            if (mode == 1)
                this.sourceform = fG_WebForm;
            else
                this.form = fG_WebForm;
            this.user = fG_User;
            this.system = fG_System;
        }

        public FG_Root(FG_Params fG_Params)
        {
            this.parameters = fG_Params;
        }
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

    public class FG_System
    {
        public List<FG_Notification> Notifications { get; set; }

        public FG_System()
        {
            this.Notifications = new List<FG_Notification>();
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
    }

    public class FG_Notification
    {
        public int UserId { get; set; }

        public List<int> RoleIds { get; set; }

        public List<int> UserGroupIds { get; set; }

        public string Title { get; set; }

        public FG_NotifyBy NotifyBy { get; set; }
    }

    public enum FG_NotifyBy
    {
        UserId = 1,
        RoleIds = 2,
        UserGroupIds = 3
    }

    public class FG_WebForm : DynamicObject
    {
        public FG_Row FlatCtrls { get; set; }

        public List<FG_DataGrid> DataGrids { get; set; }

        public FG_Review Review { get; set; }

        public int id { get; set; } // rowId

        public int eb_loc_id { get; set; }

        public string eb_ref_id { get; set; }

        public int eb_created_by { get; set; }

        public FG_WebForm()
        {
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

    }

    public class FG_DataGrid
    {
        public string Name { get; private set; }

        public List<FG_Row> Rows { get; private set; }

        public FG_DataGrid(string name, List<FG_Row> rows)
        {
            this.Name = name;
            this.Rows = rows;
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
                if (ctrl == null)
                    Console.WriteLine($"Null ref in form globals. Name = {name}, CtrlCount = {this.Controls.Count}, RowId = {this.id}");
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
    }

    public class FG_Control
    {
        public string Name { get; private set; }

        public object Value { get; private set; }

        public FG_Control(string Name, object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public object getValue()
        {
            return this.Value;
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
}
