using SQLite;

namespace Mobile.Code.Models
{
    public class VisualProjectLocation
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string ProjectLocationId { get; set; }
        public string Name { get; set; }


        public string ImageDescription { get; set; }


        public string AdditionalConsideration { get; set; }

        public string ExteriorElements { get; set; }


        public string WaterProofingElements { get; set; }
        public string ConditionAssessment { get; set; }
        public string VisualReview { get; set; }

        public string AnyVisualSign { get; set; }
        public string FurtherInasive { get; set; }
        public string LifeExpectancyEEE { get; set; }
        public string LifeExpectancyLBC { get; set; }
        public string LifeExpectancyAWE { get; set; }


        public string CreatedOn { get; set; }

        public bool IsOriginal { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public bool IsDelete { get; set; }
        public int SeqNo { get; set; }
        public string Username { get; set; }


    }
}
