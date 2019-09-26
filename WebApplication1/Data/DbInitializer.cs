using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public static class DbInitializer
    {
        public static void Initialize(WebApplication1Context context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }
            string tempAdvancedDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris id suscipit orci, sit amet auctor lorem. In luctus augue ex, at lacinia sapien aliquet vitae. Donec ut dignissim enim, egestas eleifend elit. Sed et arcu consequat, porttitor augue ut, commodo mauris. Aliquam eu ex ex. Sed ullamcorper purus magna, at aliquam tellus malesuada sed. Pellentesque rhoncus dui ac ex viverra, non scelerisque augue dictum. Donec quis sollicitudin ante. Aenean tortor nunc, finibus nec dapibus eget, vestibulum ut ex. Praesent id aliquam erat. Donec nibh diam, fermentum facilisis pellentesque et, fringilla nec nunc. Mauris condimentum tristique neque tincidunt rutrum. Sed mattis nunc ac ligula scelerisque luctus. Proin porttitor velit eget orci porttitor, nec lacinia eros fermentum.Nullam sit amet condimentum enim.Praesent scelerisque erat ac libero posuere feugiat.Phasellus tristique dignissim velit vel pharetra. Vestibulum feugiat est nec eros fringilla, vel sodales eros viverra.Fusce quam risus, fringilla consequat ipsum quis, mollis sagittis erat. Proin vestibulum feugiat mauris, molestie sodales metus sagittis nec. Phasellus luctus fermentum tellus ut convallis. Morbi rutrum libero odio, sit amet gravida metus mollis a.Aenean a ullamcorper augue, sit amet varius justo.Vivamus et blandit augue, in commodo ex. Nam arcu odio, convallis id dolor eu, ornare fringilla nisi. Integer quis urna sit amet libero elementum consequat a a lacus.Aliquam hendrerit dictum nibh, sit amet ullamcorper lorem efficitur non.Sed vitae luctus lectus. Nullam pharetra ante quis porta sodales";
            var products = new Product[]
            {
                new Product{Description="Dette er produktet til dig med ordblindhed", Price=699.99, AdvancedDescription=tempAdvancedDescription, Picture="https://dyslexiaida.org/wp-content/uploads/2016/05/Not-Stupid-Not-Lazy-Cover-Final-sm.jpg" },
                new Product{Description="Dette er produktet til dig med svagt syn", Price=1250, AdvancedDescription=tempAdvancedDescription, Picture="https://res.cloudinary.com/liingo/image/upload/c_fill,g_center,h_339,w_990,q_85/754317164787_2.jpg"},
                new Product{Description="Dette er et testprodukt", Price=69, AdvancedDescription=tempAdvancedDescription, Picture="https://images.sftcdn.net/images/t_app-cover-l,f_auto/p/ce2ece60-9b32-11e6-95ab-00163ed833e7/260663710/the-test-fun-for-friends-screenshot.jpg"}

            };
            foreach (Product p in products)
            {
                context.Products.Add(p);
            }

            // Resolving concurrency conflicts
            var saved = false;
            while (!saved)
            {
                try
                {
                    context.SaveChanges();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Product)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // TODO: decide which value should be written to database
                                proposedValues[property] = databaseValue;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
            

            
            
        }
    }
}