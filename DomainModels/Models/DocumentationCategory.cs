using System.ComponentModel.DataAnnotations;

namespace DomainModels.Models;

public enum DocumentationCategory
{
    [Display(Name = "SSH & Server Security")]
    SshAndServerSecurity,

    [Display(Name = "DNS, Firewall & Domain")]
    DnsFirewallAndDomain,

    [Display(Name = "Database Setup")]
    DatabaseSetup,

    [Display(Name = "NGINX, HTTPS & Reverse Proxy")]
    NginxHttpsAndReverseProxy,

    [Display(Name = "Docker Fundamentals")]
    DockerFundamentals,

    [Display(Name = "Docker Compose")]
    DockerCompose,

    [Display(Name = "Volumes, Persistence & Networking")]
    VolumesPersistenceAndNetworking,

    [Display(Name = "Dokploy, GitHub & CI/CD")]
    DokployGithubAndCiCd,

    [Display(Name = "Monitoring & Logging")]
    MonitoringAndLogging,

    [Display(Name = "OWASP & Security Headers")]
    OwaspAndSecurityHeaders,

    [Display(Name = "Container Security & Secrets")]
    ContainerSecurityAndSecrets,

}
