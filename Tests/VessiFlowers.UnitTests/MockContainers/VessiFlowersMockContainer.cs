namespace VessiFlowers.UnitTests.MockContainers
{
    using System.Collections.Generic;
    using Moq;
    using VessiFlowers.Common.Utilities;
    using VessiFlowers.Data.Entities;
    using VessiFlowers.Data.Repositories;

    public class VessiFlowersMockContainer : MockContainer
    {
        private Mock<InstanceFactory<IVessiFlowersContext>> contextFactory;

        private Mock<VessiFlowersContext> context;

        public VessiFlowersMockContainer()
        {
            this.SetupContextFactory();
        }

        public IVessiFlowersContext Context => this.context.Object;

        public InstanceFactory<IVessiFlowersContext> ContextFactory => this.contextFactory.Object;

        private void SetupContextFactory()
        {
            this.context = new Mock<VessiFlowersContext>();
            this.MockField<VessiFlowersContext, User>(this.context, new List<User>(), c => c.Users);
            this.contextFactory = new Mock<InstanceFactory<IVessiFlowersContext>>(this.context.Object);
            this.contextFactory.Setup(c => c.Create()).Returns(this.context.Object);
        }
    }
}
