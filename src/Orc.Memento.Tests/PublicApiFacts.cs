// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicApiFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Memento.Tests
{
    using System.Runtime.CompilerServices;
    using ApiApprover;
    using NUnit.Framework;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orc_Memento_HasNoBreakingChanges()
        {
            var assembly = typeof(MementoService).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }
    }
}