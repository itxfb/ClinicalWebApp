using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicalWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class initial9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Informativs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Decisions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "Actions",
                type: "int",
                nullable: true);

            var getproc = @"CREATE OR ALTER PROCEDURE [dbo].[GetAllRecords]
                        
                        @TblName varchar(50) = '',

						@EntityName varchar(50) ='',

                        @EntityValue varchar(50) = '-1',

						@Join varchar(MAX) ='',

						@IsActive varchar(50) ='0',

                        @row varchar(20) = '0',

                        @ofset varchar(20) = '0',

                        @SortType varchar(50) = 'Id',

                        @SortDirection varchar(50) = '',

						@count int = 0

                        AS
                        
						BEGIN     

                            DECLARE @SQL NVARCHAR(MAX)

                            SELECT @SQL = COALESCE(@SQL +','  ,' ') + name from sys.columns where name not in ('UpdatedAt','DeletedAt') and object_id = (Select id from sysobjects where name = @TblName)


                            --Get User By Id
                            IF(@EntityValue!='-1')

                                    BEGIN

                                    SELECT @SQL = 'SELECT ' + @SQL + ' FROM ' +@TblName+' WITH (NOLOCK) WHERE '+@EntityName+'=' + @EntityValue + ' AND IsActive <> 0 Order By '+@SortType+' '+@SortDirection+''

									EXEC (@SQL)

									SELECT @count = @@ROWCOUNT

									return @count

                                    END

							

							ELSE IF(@Join != '')

									BEGIN 

										 SELECT @SQL = 'SELECT '+@SQL+' FROM ' +@TblName+' WITH (NOLOCK) '+@Join+''

										 EXEC (@SQL)

									END

                            ELSE IF(@row!='0')
                                    
                                    BEGIN

                                    SELECT @SQL = 'SELECT ' + @SQL + ' FROM '+@TblName+' WITH (NOLOCK) WHERE IsActive <> 0 Order By '+@SortType+' '+@SortDirection+' OFFSET '+@ofset+' ROWS FETCH NEXT '+@row+' ROWS ONLY'
 
									EXEC (@SQL)

                                    END

							ELSE IF(@IsActive!='0')


								BEGIN 
										SELECT @SQL = 'SELECT ' + @SQL + ' FROM '+@TblName+' WITH (NOLOCK) WHERE IsActive ='+@IsActive+''
										
										EXEC (@SQL)
							
								END


                            ELSE

                                    BEGIN 

                                        SELECT @SQL = 'SELECT ' + @SQL + ' FROM '+@TblName+' WITH (NOLOCK) WHERE IsActive <> 0'
										
										EXEC (@SQL)
                                    END



                        END";

            var insertOrUpdate = @"CREATE OR ALTER PROCEDURE [dbo].[InsertOrUpdate]
                    
                     @Id varchar(MAX) = '0',

                     @TblName varchar(MAX)='',

                     @Columns varchar(MAX) = '0',

                     @Values varchar(MAX) = ''

                        AS
                        
                         Declare @insert NVARCHAR(MAX) ='Insert Into '+@TblName+' ('+@Columns+') Values ('+@Values+')'
                        
                         Declare @update NVARCHAR(MAX) ='UPDATE '+@TblName+' SET '+@Columns+' where Id = '+@Id;

						 
                        BEGIN
                  
                    IF(@Id = 0)

                     BEGIN

					
					 EXEC(@insert)
					 
					
                      --EXECUTE sp_executesql @insert
					  SELECT Id= @@IDENTITY

		             END

                ELSE


                BEGIN

				 EXEC(@update)
                    --EXECUTE sp_executesql @update

                END

               
                END";

            migrationBuilder.Sql(getproc);
            migrationBuilder.Sql(insertOrUpdate);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Informativs");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Actions");
        }
    }
}
