namespace Stashbox.Web.WebForms
{
    public interface IStashboxWebFormsConfiguration
    {
        bool TrackUnresolvableTypes { get; set; }
        int UnresolvableTypeTrackingLimit { get; set; }
        bool CallNextActivator { get; set; }
    }
}
