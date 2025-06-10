namespace Asset_management.models
{
    public interface IAsset
    {
        int Id { get; set; }
        string Location { get; set; }
        DateTime AcquisitionDate { get; set; }
    }

}
