namespace Stashbox.Web.WebForms
{
    public class StashboxWebFormsConfiguration : IStashboxWebFormsConfiguration
    {
        public bool TrackUnresolvableTypes { get; set; }

        public int UnresolvableTypeTrackingLimit { get; set; }

        public bool CallNextActivator { get; set; }
    }
}