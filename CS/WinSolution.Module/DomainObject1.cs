using System;

using DevExpress.Xpo;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;

namespace WinSolution.Module {
    [DefaultClassOptions]
    public class DomainObject1 : BaseObject {
        public DomainObject1(Session session) : base(session) { }
        private string _name;
        public string Name {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }
        
        private int _age;
        public int Age {
            get { return _age; }
            set { SetPropertyValue("Age", ref _age, value); }
        }
        private string _description;
        public string Description
        {
        	get {	return _description; }
        	set { SetPropertyValue("Description", ref _description, value);	}
        }
        private DomainObject1 _ReferencedProperty;
        public DomainObject1 ReferencedProperty {
            get { return _ReferencedProperty; }
            set { SetPropertyValue("ReferencedProperty", ref _ReferencedProperty, value); }
        }
    }
}
