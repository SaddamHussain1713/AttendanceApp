namespace AttendanceSystem.Domain
{
    public class ClassEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public List<Student> Students { get; set; } = [];
        public List<ClassSession> ClassSessions { get; set; } = [];
    }
}
