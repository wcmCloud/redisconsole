using Microsoft.EntityFrameworkCore.Migrations;

namespace Redis.Core.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Instances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Host = table.Column<string>(type: "TEXT", nullable: false),
                    Port = table.Column<int>(type: "INTEGER", nullable: false),
                    Auth = table.Column<string>(type: "TEXT", nullable: true),
                    SslEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    SslLocalCertPath = table.Column<string>(type: "TEXT", nullable: true),
                    SslPrivateKeyPath = table.Column<string>(type: "TEXT", nullable: true),
                    SslCaCertPath = table.Column<string>(type: "TEXT", nullable: true),
                    SshPassword = table.Column<string>(type: "TEXT", nullable: true),
                    SshUser = table.Column<string>(type: "TEXT", nullable: true),
                    SshHost = table.Column<string>(type: "TEXT", nullable: true),
                    SshPort = table.Column<int>(type: "INTEGER", nullable: false),
                    SshPrivateKey = table.Column<string>(type: "TEXT", nullable: true),
                    KeysPattern = table.Column<string>(type: "TEXT", nullable: true),
                    NamespaceSeparator = table.Column<string>(type: "TEXT", nullable: true),
                    ExecuteTimeout = table.Column<int>(type: "INTEGER", nullable: false),
                    ConnectionTimeout = table.Column<int>(type: "INTEGER", nullable: false),
                    LuaKeysLoading = table.Column<bool>(type: "INTEGER", nullable: false),
                    OverrideClusterHost = table.Column<bool>(type: "INTEGER", nullable: false),
                    IgnoreSSLErrors = table.Column<bool>(type: "INTEGER", nullable: false),
                    DatabaseScanLimit = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instances", x => x.Id);
                    table.UniqueConstraint("UK_Instances_Name", x => x.Name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instances");
        }
    }
}
