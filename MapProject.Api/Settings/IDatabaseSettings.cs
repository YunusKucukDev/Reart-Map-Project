namespace MapProject.Api.Settings
{
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public string MapIdentityDescriptionCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ContactCollectionName { get; set; }
        public string UserInformationCollectionName { get; set; }

        public string UsersIdentityCollectionName { get; set; }
        public string RolesIdentityCollectionName { get; set; }
        public string VisitorLogCollectionName { get; set; }
    }
}
