﻿using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponID = 1,
                CouponCode = "10OFF",
                DiscountAmount = 10,
                MinAmount = 20
            });
            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                CouponID = 2,
                CouponCode = "20OFF",
                DiscountAmount = 20,
                MinAmount = 40
            });

        }
    }
}
