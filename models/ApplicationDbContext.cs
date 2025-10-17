using Microsoft.EntityFrameworkCore;

namespace Asset_management.models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<StandardAsset> StandardAssets { get; set; }
        public DbSet<FurnitureFitting> FurnitureFittings { get; set; }
        public DbSet<PlantMachinery> PlantMachineries { get; set; }
        public DbSet<PortableItem> PortableItems { get; set; }
        public DbSet<MotorVehicle> MotorVehicles { get; set; }
        public DbSet<LandRegister> LandRegisters { get; set; }
        public DbSet<BuildingsRegister> BuildingsRegisters { get; set; }
        public DbSet<IntangibleAsset> IntangibleAssets { get; set; }
        public DbSet<StocksRegister> StocksRegisters { get; set; }
        public DbSet<RoadsInfrastructure> RoadsInfrastructures { get; set; }
        public DbSet<OtherInfrastructure> OtherInfrastructures { get; set; }
        public DbSet<BiologicalAsset> BiologicalAssets { get; set; }
        public DbSet<SubsoilAsset> SubsoilAssets { get; set; }
        public DbSet<MajorMaintenance> MajorMaintenances { get; set; }
        public DbSet<WorkInProgress> WorkInProgresses { get; set; }
        public DbSet<Investments> Investments { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<AccountsReceivable> AccountsReceivables { get; set; }
        public DbSet<OtherReceivable> OtherReceivables { get; set; }
        public DbSet<Imprest> Imprests { get; set; }
        public DbSet<AccountsPayable> AccountsPayables { get; set; }
        public DbSet<AssetMovement> AssetMovements { get; set; }
       // public DbSet<Revaluation> Revaluations { get; set; }
        public DbSet<LossesRegister> LossesRegisters { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<Litigation> Litigations { get; set; }
        public DbSet<EquipmentSignout> EquipmentSignouts { get; set; }
        public DbSet<AssetTransfer> AssetTransfers { get; set; }
        public DbSet<AssetHandover> AssetHandovers { get; set; }
        public DbSet<AssetReconciliation> AssetReconciliations { get; set; }

    }
}
