using TechnosoftModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Models
{
    public class ComboCourseModel
    {
      public int  ID { get; set; }
      public int  COURSEID { get; set; }
    public int INSTITUTEID{ get; set; }
    public string CODE { get; set; }
	public string COURSETYPE{ get; set; }
	public string COURSENAME { get; set; }
	public string DURATION { get; set; }
	public string COURSEDESCRIPTION { get; set; }
	public int COURSESTATUS { get; set; }
    public string COURSEPATTERN{ get; set; }
        public IEnumerable<SubcoursesOfCombo_Result> SubCourses { get; set; }
    }
}