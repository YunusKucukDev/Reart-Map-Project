namespace MapProject.WebUI.Services.VisitorService
{
    public class VisitorTracker
    {
        private readonly HashSet<string> _visitors = new();
        private readonly object _lock = new();

        public int Count
        {
            get { lock (_lock) return _visitors.Count; }
        }

        public void Add(string connectionId)
        {
            lock (_lock) _visitors.Add(connectionId);
        }

        public bool Remove(string connectionId)
        {
            lock (_lock) return _visitors.Remove(connectionId);
        }

        public bool Contains(string connectionId)
        {
            lock (_lock) return _visitors.Contains(connectionId);
        }
    }
}
