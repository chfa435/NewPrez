using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NewTiceAI.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialReset040525 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionItemPriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionItemPriorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionItemStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionItemStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: true),
                    ZipCode = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileUploads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Imports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImportDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevertDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ImportComment = table.Column<string>(type: "text", nullable: true),
                    RevertReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPriorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketPriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPriorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AddressId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentOrganizationId = table.Column<int>(type: "integer", nullable: true),
                    BillingAddressId = table.Column<int>(type: "integer", nullable: true),
                    ShippingAddressId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AnnualRevenue = table.Column<decimal>(type: "numeric", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Website = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Addresses_BillingAddressId",
                        column: x => x.BillingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Accounts_Addresses_ShippingAddressId",
                        column: x => x.ShippingAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Accounts_Organizations_ParentOrganizationId",
                        column: x => x.ParentOrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActionProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    ProjectPriorityId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageFileName = table.Column<string>(type: "text", nullable: true),
                    ImageFileData = table.Column<byte[]>(type: "bytea", nullable: true),
                    ImageContentType = table.Column<string>(type: "text", nullable: true),
                    Archived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionProjects_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionProjects_ProjectPriorities_ProjectPriorityId",
                        column: x => x.ProjectPriorityId,
                        principalTable: "ProjectPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    ProjectPriorityId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageFileName = table.Column<string>(type: "text", nullable: true),
                    ImageFileData = table.Column<byte[]>(type: "bytea", nullable: true),
                    ImageContentType = table.Column<string>(type: "text", nullable: true),
                    Archived = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projects_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_ProjectPriorities_ProjectPriorityId",
                        column: x => x.ProjectPriorityId,
                        principalTable: "ProjectPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    LastName = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    AvatarName = table.Column<string>(type: "text", nullable: true),
                    AvatarData = table.Column<byte[]>(type: "bytea", nullable: true),
                    AvatarContentType = table.Column<string>(type: "text", nullable: true),
                    AvatarId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActionProjectId = table.Column<int>(type: "integer", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_ActionProjects_ActionProjectId",
                        column: x => x.ActionProjectId,
                        principalTable: "ActionProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_FileUploads_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "FileUploads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountTAUser",
                columns: table => new
                {
                    AccountMembersId = table.Column<string>(type: "text", nullable: false),
                    AccountsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTAUser", x => new { x.AccountMembersId, x.AccountsId });
                    table.ForeignKey(
                        name: "FK_AccountTAUser_Accounts_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountTAUser_AspNetUsers_AccountMembersId",
                        column: x => x.AccountMembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InviteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrganizationToken = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    InvitorId = table.Column<string>(type: "text", nullable: false),
                    InviteeId = table.Column<string>(type: "text", nullable: true),
                    InviteeEmail = table.Column<string>(type: "text", nullable: false),
                    InviteeFirstName = table.Column<string>(type: "text", nullable: false),
                    InviteeLastName = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invites_AspNetUsers_InviteeId",
                        column: x => x.InviteeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invites_AspNetUsers_InvitorId",
                        column: x => x.InvitorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invites_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invites_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invites_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTAUser",
                columns: table => new
                {
                    MembersId = table.Column<string>(type: "text", nullable: false),
                    ProjectsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTAUser", x => new { x.MembersId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_ProjectTAUser_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTAUser_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubmitterUserId = table.Column<string>(type: "text", nullable: false),
                    DeveloperUserId = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    AccountId = table.Column<int>(type: "integer", nullable: true),
                    TicketTypeId = table.Column<int>(type: "integer", nullable: false),
                    TicketStatusId = table.Column<int>(type: "integer", nullable: false),
                    TicketPriorityId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Archived = table.Column<bool>(type: "boolean", nullable: false),
                    ArchivedByProject = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_DeveloperUserId",
                        column: x => x.DeveloperUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_SubmitterUserId",
                        column: x => x.SubmitterUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_TicketPriorities_TicketPriorityId",
                        column: x => x.TicketPriorityId,
                        principalTable: "TicketPriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketStatuses_TicketStatusId",
                        column: x => x.TicketStatusId,
                        principalTable: "TicketStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    FileData = table.Column<byte[]>(type: "bytea", nullable: true),
                    FileContentType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketAttachments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketAttachments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketComments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketHistories_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketTicketTag",
                columns: table => new
                {
                    TagsId = table.Column<int>(type: "integer", nullable: false),
                    TicketsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTicketTag", x => new { x.TagsId, x.TicketsId });
                    table.ForeignKey(
                        name: "FK_TicketTicketTag_TicketTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "TicketTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketTicketTag_Tickets_TicketsId",
                        column: x => x.TicketsId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionItemAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActionItemId = table.Column<int>(type: "integer", nullable: false),
                    SubmitterId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    FileData = table.Column<byte[]>(type: "bytea", nullable: true),
                    FileContentType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionItemAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionItemAttachments_AspNetUsers_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionItemComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActionItemId = table.Column<int>(type: "integer", nullable: false),
                    SubmitterId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionItemComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionItemComments_AspNetUsers_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionItemHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PropertyName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    ActionItemId = table.Column<int>(type: "integer", nullable: false),
                    SubmitterId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionItemHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionItemHistories_AspNetUsers_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactId = table.Column<int>(type: "integer", nullable: true),
                    SubmitterId = table.Column<string>(type: "text", nullable: false),
                    ActorId = table.Column<string>(type: "text", nullable: true),
                    AccountId = table.Column<int>(type: "integer", nullable: true),
                    ActionProjectId = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: true),
                    ItemStatus = table.Column<int>(type: "integer", nullable: true),
                    ItemPriority = table.Column<int>(type: "integer", nullable: true),
                    OpportunityType = table.Column<int>(type: "integer", nullable: true),
                    OpportunityForecastCategory = table.Column<int>(type: "integer", nullable: true),
                    OpportunityStage = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Archived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionItems_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionItems_ActionProjects_ActionProjectId",
                        column: x => x.ActionProjectId,
                        principalTable: "ActionProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionItems_AspNetUsers_ActorId",
                        column: x => x.ActorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActionItems_AspNetUsers_SubmitterId",
                        column: x => x.SubmitterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    TicketId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    RecipientId = table.Column<string>(type: "text", nullable: true),
                    NotificationTypeId = table.Column<int>(type: "integer", nullable: false),
                    Viewed = table.Column<bool>(type: "boolean", nullable: false),
                    ActionItemId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_ActionItems_ActionItemId",
                        column: x => x.ActionItemId,
                        principalTable: "ActionItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ContactId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    FileData = table.Column<byte[]>(type: "bytea", nullable: true),
                    FileContentType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactAttachments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: true),
                    AddressId = table.Column<int>(type: "integer", nullable: true),
                    ContactOwnerId = table.Column<string>(type: "text", nullable: true),
                    ImportId = table.Column<int>(type: "integer", nullable: true),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Mobile = table.Column<string>(type: "text", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    DoNotCall = table.Column<bool>(type: "boolean", nullable: false),
                    EmailOptOut = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    Specialty = table.Column<string>(type: "text", nullable: true),
                    ResidencyId = table.Column<int>(type: "integer", nullable: true),
                    Residency_GradYear = table.Column<int>(type: "integer", nullable: true),
                    FellowshipId = table.Column<int>(type: "integer", nullable: true),
                    Fellowship_GradYear = table.Column<int>(type: "integer", nullable: true),
                    Fellowship2Id = table.Column<int>(type: "integer", nullable: true),
                    Fellowship2_GradYear = table.Column<int>(type: "integer", nullable: true),
                    RelationshipHolderId = table.Column<int>(type: "integer", nullable: true),
                    CurrentDistributorId = table.Column<int>(type: "integer", nullable: true),
                    SalesRepresentativeId = table.Column<int>(type: "integer", nullable: true),
                    PracticeId = table.Column<int>(type: "integer", nullable: true),
                    MentorId = table.Column<int>(type: "integer", nullable: true),
                    LastMeetingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextActivityDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FollowupDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HandoffConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    ContactNotes = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true),
                    ProfileUrl = table.Column<string>(type: "text", nullable: true),
                    ProfileUrl2 = table.Column<string>(type: "text", nullable: true),
                    PacketSentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contacts_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contacts_AspNetUsers_ContactOwnerId",
                        column: x => x.ContactOwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contacts_Contacts_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contacts_Contacts_RelationshipHolderId",
                        column: x => x.RelationshipHolderId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contacts_Contacts_SalesRepresentativeId",
                        column: x => x.SalesRepresentativeId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contacts_FileUploads_ImageId",
                        column: x => x.ImageId,
                        principalTable: "FileUploads",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contacts_Imports_ImportId",
                        column: x => x.ImportId,
                        principalTable: "Imports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Contacts_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Institutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AddressId = table.Column<int>(type: "integer", nullable: true),
                    ContactId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Institutions_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Institutions_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountTAUser_AccountsId",
                table: "AccountTAUser",
                column: "AccountsId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BillingAddressId",
                table: "Accounts",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentOrganizationId",
                table: "Accounts",
                column: "ParentOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ShippingAddressId",
                table: "Accounts",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItemAttachments_ActionItemId",
                table: "ActionItemAttachments",
                column: "ActionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItemAttachments_SubmitterId",
                table: "ActionItemAttachments",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItemComments_ActionItemId",
                table: "ActionItemComments",
                column: "ActionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItemComments_SubmitterId",
                table: "ActionItemComments",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItemHistories_ActionItemId",
                table: "ActionItemHistories",
                column: "ActionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItemHistories_SubmitterId",
                table: "ActionItemHistories",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_AccountId",
                table: "ActionItems",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_ActionProjectId",
                table: "ActionItems",
                column: "ActionProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_ActorId",
                table: "ActionItems",
                column: "ActorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_ContactId",
                table: "ActionItems",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_SubmitterId",
                table: "ActionItems",
                column: "SubmitterId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionProjects_AccountId",
                table: "ActionProjects",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionProjects_ProjectPriorityId",
                table: "ActionProjects",
                column: "ProjectPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ActionProjectId",
                table: "AspNetUsers",
                column: "ActionProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AvatarId",
                table: "AspNetUsers",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrganizationId",
                table: "AspNetUsers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactAttachments_ContactId",
                table: "ContactAttachments",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAttachments_UserId",
                table: "ContactAttachments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AccountId",
                table: "Contacts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AddressId",
                table: "Contacts",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ContactOwnerId",
                table: "Contacts",
                column: "ContactOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CurrentDistributorId",
                table: "Contacts",
                column: "CurrentDistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Fellowship2Id",
                table: "Contacts",
                column: "Fellowship2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_FellowshipId",
                table: "Contacts",
                column: "FellowshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ImageId",
                table: "Contacts",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ImportId",
                table: "Contacts",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_MentorId",
                table: "Contacts",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_OrganizationId",
                table: "Contacts",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PracticeId",
                table: "Contacts",
                column: "PracticeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_RelationshipHolderId",
                table: "Contacts",
                column: "RelationshipHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ResidencyId",
                table: "Contacts",
                column: "ResidencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_SalesRepresentativeId",
                table: "Contacts",
                column: "SalesRepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_AddressId",
                table: "Institutions",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Institutions_ContactId",
                table: "Institutions",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_CompanyId",
                table: "Invites",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_InviteeId",
                table: "Invites",
                column: "InviteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_InvitorId",
                table: "Invites",
                column: "InvitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_OrganizationId",
                table: "Invites",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_ProjectId",
                table: "Invites",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ActionItemId",
                table: "Notifications",
                column: "ActionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ProjectId",
                table: "Notifications",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecipientId",
                table: "Notifications",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TicketId",
                table: "Notifications",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_AddressId",
                table: "Organizations",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTAUser_ProjectsId",
                table: "ProjectTAUser",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AccountId",
                table: "Projects",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CompanyId",
                table: "Projects",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectPriorityId",
                table: "Projects",
                column: "ProjectPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_TicketId",
                table: "TicketAttachments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketAttachments_UserId",
                table: "TicketAttachments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_TicketId",
                table: "TicketComments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_UserId",
                table: "TicketComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketId",
                table: "TicketHistories",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_UserId",
                table: "TicketHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTicketTag_TicketsId",
                table: "TicketTicketTag",
                column: "TicketsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_AccountId",
                table: "Tickets",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DeveloperUserId",
                table: "Tickets",
                column: "DeveloperUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectId",
                table: "Tickets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SubmitterUserId",
                table: "Tickets",
                column: "SubmitterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketPriorityId",
                table: "Tickets",
                column: "TicketPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketStatusId",
                table: "Tickets",
                column: "TicketStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketTypeId",
                table: "Tickets",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionItemAttachments_ActionItems_ActionItemId",
                table: "ActionItemAttachments",
                column: "ActionItemId",
                principalTable: "ActionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionItemComments_ActionItems_ActionItemId",
                table: "ActionItemComments",
                column: "ActionItemId",
                principalTable: "ActionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionItemHistories_ActionItems_ActionItemId",
                table: "ActionItemHistories",
                column: "ActionItemId",
                principalTable: "ActionItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionItems_Contacts_ContactId",
                table: "ActionItems",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactAttachments_Contacts_ContactId",
                table: "ContactAttachments",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Institutions_CurrentDistributorId",
                table: "Contacts",
                column: "CurrentDistributorId",
                principalTable: "Institutions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Institutions_Fellowship2Id",
                table: "Contacts",
                column: "Fellowship2Id",
                principalTable: "Institutions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Institutions_FellowshipId",
                table: "Contacts",
                column: "FellowshipId",
                principalTable: "Institutions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Institutions_PracticeId",
                table: "Contacts",
                column: "PracticeId",
                principalTable: "Institutions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Institutions_ResidencyId",
                table: "Contacts",
                column: "ResidencyId",
                principalTable: "Institutions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionProjects_Accounts_AccountId",
                table: "ActionProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Accounts_AccountId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_AspNetUsers_ContactOwnerId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Addresses_AddressId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Institutions_Addresses_AddressId",
                table: "Institutions");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Addresses_AddressId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Organizations_OrganizationId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Institutions_Contacts_ContactId",
                table: "Institutions");

            migrationBuilder.DropTable(
                name: "AccountTAUser");

            migrationBuilder.DropTable(
                name: "ActionItemAttachments");

            migrationBuilder.DropTable(
                name: "ActionItemComments");

            migrationBuilder.DropTable(
                name: "ActionItemHistories");

            migrationBuilder.DropTable(
                name: "ActionItemPriorities");

            migrationBuilder.DropTable(
                name: "ActionItemStatuses");

            migrationBuilder.DropTable(
                name: "ActionItemTypes");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ContactAttachments");

            migrationBuilder.DropTable(
                name: "Invites");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ProjectTAUser");

            migrationBuilder.DropTable(
                name: "TicketAttachments");

            migrationBuilder.DropTable(
                name: "TicketComments");

            migrationBuilder.DropTable(
                name: "TicketHistories");

            migrationBuilder.DropTable(
                name: "TicketTicketTag");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ActionItems");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "TicketTags");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TicketPriorities");

            migrationBuilder.DropTable(
                name: "TicketStatuses");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ActionProjects");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "ProjectPriorities");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "FileUploads");

            migrationBuilder.DropTable(
                name: "Imports");

            migrationBuilder.DropTable(
                name: "Institutions");
        }
    }
}
