//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MRS.Module.BusinessObjects
{
    using System;
    using System.Collections.Generic;
    
    public partial class HCategory
    {
        public HCategory()
        {
            this.Children = new List<HCategory>();
        }
    
        private int _iD;
    	public int ID 
    	{ 
    		get { return _iD; } 
    		protected set
    		{
    			if (value != _iD) {
    				_iD = value;
    				 OnIDChanged();
    			}
    		} 
    	}
    	partial void OnIDChanged(); 
    
        private string _name;
    	public string Name 
    	{ 
    		get { return _name; } 
    		set
    		{
    			if (value != _name) {
    				_name = value;
    				 OnNameChanged();
    			}
    		} 
    	}
    	partial void OnNameChanged(); 
    
    
        public virtual HCategory Parent { get; set; }
        public virtual ICollection<HCategory> Children { get; set; }
    }
}