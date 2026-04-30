namespace MapProject.Api.Settings
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public string MapIdentityDescriptionCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ContactCollectionName { get; set; }
        public string UserInformationCollectionName { get; set; }
        public string CoureselCollectionName { get; set; }
        public string MapViewerCollectionName { get; set; }

        public string UsersIdentityCollectionName { get; set; }
        public string RolesIdentityCollectionName { get; set; }
        public string VisitorLogCollectionName { get; set; }
        public string VideoCollectionName { get; set; }
    }
}
