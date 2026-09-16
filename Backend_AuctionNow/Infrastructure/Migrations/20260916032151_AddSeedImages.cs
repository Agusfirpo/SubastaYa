using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1,
                column: "UrlImagen",
                value: "https://www.oscarbarbieri.com/media/catalog/product/cache/7baadf0dec41407c7702efdbff940ecb/s/a/samsung_book3_pro_1c.jpg");

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                column: "UrlImagen",
                value: "https://i.ytimg.com/vi/8nRmAnVcrr8/sddefault.jpg");

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                column: "UrlImagen",
                value: "https://www.lanacion.com.ar/resizer/v2/el-renault-clio-CA2KPAI3QJDGBLEJTL2XFQWPCU.jpg?auth=af6607c206e76ead75a309fe7b2a02a3a1c877929bb752bf324e0f0f070b95f9&width=420&height=280&quality=70&smart=true");

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                column: "UrlImagen",
                value: "https://www.lg.com/ar/images/monitores/md05985257/gallery/medium02.jpg");

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                column: "UrlImagen",
                value: "https://acdn-us.mitiendanube.com/stores/591/146/products/campera-cuero-moto-negan-frente-liam-leather-cdcfcdf3f6704e553817277234154959-1024-1024.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 1,
                column: "UrlImagen",
                value: "img1.png");

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 2,
                column: "UrlImagen",
                value: "img2.png");

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 3,
                column: "UrlImagen",
                value: "img3.png");

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 4,
                column: "UrlImagen",
                value: "img4.png");

            migrationBuilder.UpdateData(
                table: "Subastas",
                keyColumn: "Id",
                keyValue: 5,
                column: "UrlImagen",
                value: "img5.png");
        }
    }
}
