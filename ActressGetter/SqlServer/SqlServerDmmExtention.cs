using Dapper;
using FanzaActressSearch.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ActressGetter.SqlServer
{
    public static class SqlServerDmmExtention
    {
        public static Actress SingleOrDefaultActress(this SqlConnection sqlConnection, int id)
            => sqlConnection.Query<Actress>($"select * from Actress Where Id = {id}").SingleOrDefault();

        public static IEnumerable<Product> SelectProduct(this SqlConnection sqlConnection, int actressId)
            => sqlConnection.Query<Product>($"select P.Id from Actress AS A join ActressProduct as AP ON AP.ActressID = A.Id join Product as P ON AP.ProductID = P.Id Where A.Id = '{actressId}'");

        public static void InsertOrUpdateActress(this SqlConnection sqlConnection, Actress actress, DateTime now)
        {
            var single = sqlConnection.Query<Actress>($"select * from Actress Where Id = '{actress.Id}'").SingleOrDefault();
            if (single != null)
            {
                actress.CreateDate = single.CreateDate;
                actress.UpdateDate = now;
                sqlConnection.Update(actress, nameof(actress.Id));
            }
            else
            {
                actress.CreateDate = now;
                actress.UpdateDate = now;
                sqlConnection.Insert(actress);
            }
        }

        public static void InsertOrUpdateProduct(this SqlConnection sqlConnection, Product product, int actressId, DateTime now)
        {
            var single = sqlConnection.Query<Product>($"select * from Product Where Id = '{product.Id}'").SingleOrDefault();
            if (single != null)
            {
                product.CreateDate = single.CreateDate;
                product.UpdateDate = now;
                sqlConnection.Update(product, nameof(product.Id));
            }
            else
            {
                product.CreateDate = now;
                product.UpdateDate = now;
                sqlConnection.Insert(product);
            }
            var actressProduct = sqlConnection.Query<ActressProduct>($"select * from ActressProduct Where ActressID = '{actressId}' and ProductID = '{product.Id}'").SingleOrDefault();
            if (actressProduct == null)  sqlConnection.Insert(new ActressProduct() { ActressID = actressId, ProductID = product.Id });
        }

        public static void InsertActressProduct(this SqlConnection sqlConnection, int actressId, string productId)
        {
            sqlConnection.Insert(new ActressProduct() { ActressID = actressId, ProductID = productId });
        }

        public static void DeleteProduct(this SqlConnection sqlConnection, Product product)
        {
            sqlConnection.Execute("DELETE FROM Product WHERE Id = @Id", new { Id = product.Id });
            sqlConnection.Execute("DELETE FROM ActressProduct WHERE ProductID = @ProductID", new { ProductID = product.Id });
        }
    }
}
