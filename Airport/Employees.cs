//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Airport
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employees
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employees()
        {
            this.Box_Offic = new HashSet<Box_Offic>();
            this.EmployeesPhoto = new HashSet<EmployeesPhoto>();
        }
    
        public int id_employee { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string patronomic { get; set; }
        public int id_gender { get; set; }
        public System.DateTime date_of_birth { get; set; }
        public string phone { get; set; }
        public string login { get; set; }
        public int password { get; set; }
        public int id_role { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Box_Offic> Box_Offic { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Roles Roles { get; set; }
        public virtual Passport_deta Passport_deta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeesPhoto> EmployeesPhoto { get; set; }
    }
}