using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        //Here we create a property by typing prop + tab as shortcut
        //If the primary key is defined with different name then we can use [Key] function at the top of category but here the name is Id so the entity framework will identify this as primary key by default also CategoryId will also be treated as primary key as the name is same so no data annotation needed
        //[Key]
        public int Id { get; set; }

        [Required]
        /* [Required] - when the SQL script is generated for this table in DB the string will have not null setting
        Here we can add Data Annotation for client side UI/validation bcz otherwise it will show the below names as DisplayOrder and Name in UI
        With DisplayName, we can display the name of data which will show up in UI */

        //The MaxLength here is for giving the length of name allowed for a category
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        /* We have to add validation as well where we are adding category, previously in .net we had to use Javascript to add the validation but now in .Net core, it is simple 
           and validations are build in and the support of JQuery is added in the project.
           This will be the server side validation. So, range means the display order should be in the range 1 to 100 and it should not be zero. */
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
