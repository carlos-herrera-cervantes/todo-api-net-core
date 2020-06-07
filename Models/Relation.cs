namespace TodoApiNet.Models
{
    public class Relation
    {
        #region snippet_Properties

        public string Entity { get; set; }

        public string LocalKey { get; set; }

        public string ForeignKey { get; set; }

        #endregion

        #region snippet_Deconstruct

        public void Deconstruct(out string localKey, out string foreignKey)
        {
            localKey = LocalKey;
            foreignKey = ForeignKey;
        }

        #endregion
    }
}