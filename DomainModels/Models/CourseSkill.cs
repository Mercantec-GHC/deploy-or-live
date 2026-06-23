
public class CourseSkill
{
    public string Name { get; set; } = string.Empty;
    public bool IsAchieved { get; set; }

    public CourseSkill(string name, bool isAchieved)
    {
        Name = name;
        IsAchieved = isAchieved;
    }
}

public static class CourseSkills
{
    public static readonly IReadOnlyList<CourseSkill> All = new List<CourseSkill>
    {
        new("SSH & Server Security", true),
        new("DNS, Firewall & Domain", true),
        new("Database Setup", true),
        new("Nginx, HTTPS & Reverse Proxy", true),
        new("Docker Fundamentals", true),
        new("Docker Compose", true),
        new("Volumes, Persistence & Networking", true),
        new("Dokploy, GitHub & CI/CD", true),
        new("Monitoring & Logging", true),
        new("OWASP & Security Headers", true),
        new("Container Security & Secrets", true),
        new("Kubernetes & Terraform", false),
        new("Cloudflare Configuration", false)
    };
}
