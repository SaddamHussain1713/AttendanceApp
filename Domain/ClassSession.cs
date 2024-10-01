namespace AttendanceSystem.Domain
{
    public class ClassSession
    {
        public int Id { get; set; }               
        public required DateTime SessionDate { get; set; } = DateTime.Now.ToLocalTime();
        public int ClassId { get; set; }
        public ClassEntity? ClassEntity { get; set; }           
        public List<AttendanceRecord> AttendanceRecords { get; set; } = [];
    }
}
