﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicsLibrary.Data.Migrations
{
    public partial class AlterProcedure_GetSeriesWithBooks_AddNumberColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetSeriesWithBooks);
            migrationBuilder.Sql(StoredProcedures.Create_GetSeriesWithBooks_v3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(StoredProcedures.Drop_GetSeriesWithBooks);
            migrationBuilder.Sql(StoredProcedures.Create_GetSeriesWithBooks_v2);
        }
    }
}
