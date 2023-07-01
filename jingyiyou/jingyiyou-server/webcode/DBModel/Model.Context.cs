﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ncc2019
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ncc2019Entities : DbContext
    {
        public ncc2019Entities()
            : base("name=ncc2019Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Address> Address { get; set; }
        public DbSet<GoodSort> GoodSort { get; set; }
        public DbSet<LoginLog> LoginLog { get; set; }
        public DbSet<Info> Info { get; set; }
        public DbSet<FeedBack> FeedBack { get; set; }
        public DbSet<GetPassword> GetPassword { get; set; }
        public DbSet<PayLog> PayLog { get; set; }
        public DbSet<ActionLog> ActionLog { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<GoodProperty> GoodProperty { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<CommonLog> CommonLog { get; set; }
        public DbSet<GoodRight> GoodRight { get; set; }
        public DbSet<KuaiDiSet> KuaiDiSet { get; set; }
        public DbSet<CommonPay> CommonPay { get; set; }
        public DbSet<CFJQrCode> CFJQrCode { get; set; }
        public DbSet<CFJMemberType> CFJMemberType { get; set; }
        public DbSet<CFJPay> CFJPay { get; set; }
        public DbSet<GoodSortMapping> GoodSortMapping { get; set; }
        public DbSet<Comments_copy> Comments_copy { get; set; }
        public DbSet<ZGGKey> ZGGKey { get; set; }
        public DbSet<ZGGMachine> ZGGMachine { get; set; }
        public DbSet<ZGGPay> ZGGPay { get; set; }
        public DbSet<ZGGUseControl> ZGGUseControl { get; set; }
        public DbSet<CFJControl> CFJControl { get; set; }
        public DbSet<CFJMachine> CFJMachine { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Members> Members { get; set; }
        public DbSet<ZGGLocation> ZGGLocation { get; set; }
        public DbSet<ZGGBBS> ZGGBBS { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Apply> Apply { get; set; }
        public DbSet<Pin> Pin { get; set; }
        public DbSet<OnLineVersion> OnLineVersion { get; set; }
        public DbSet<QuanZi> QuanZi { get; set; }
        public DbSet<XiaoQu> XiaoQu { get; set; }
        public DbSet<GongGao> GongGao { get; set; }
        public DbSet<TongZhi> TongZhi { get; set; }
        public DbSet<ShangJia> ShangJia { get; set; }
        public DbSet<CarCall> CarCall { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Channel> Channel { get; set; }
        public DbSet<VIPUser> VIPUser { get; set; }
        public DbSet<Lottery> Lottery { get; set; }
        public DbSet<Seller> Seller { get; set; }
        public DbSet<NCCLottery> NCCLottery { get; set; }
        public DbSet<NCCOrders> NCCOrders { get; set; }
        public DbSet<LotterySellerRef> LotterySellerRef { get; set; }
        public DbSet<KeFuMessage> KeFuMessage { get; set; }
        public DbSet<Guide> Guide { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<vw_Orders> vw_Orders { get; set; }
    }
}
