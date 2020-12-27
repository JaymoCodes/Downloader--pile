﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Downloader.Test
{
    [TestClass]
    public class ChunkDownloaderTest : ChunkDownloader
    {
        public ChunkDownloaderTest() 
            : base(new MemoryChunk(0, 10000), 1024)
        {}

        public ChunkDownloaderTest(Chunk chunk, int blockSize) 
            : base(chunk, blockSize)
        { }

        protected override Task ReadStream(Stream stream, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private Exception GetException()
        {
            try
            {
                throw new Exception("High level exception", new IOException("Mid level exception", new HttpRequestException("Low level exception")));
            }
            catch (Exception e)
            {
                return e;
            }
        }

        [TestMethod]
        public void HasSourceFromThisNamespaceTest()
        {
            // arrange
            var exception = GetException();

            // act
            var hasThisNamespace = HasSource(exception, GetType().Namespace);

            // assert
            Assert.IsTrue(hasThisNamespace);
        }

        [TestMethod]
        public void HasSourceFromNonOccurrenceNamespaceTest()
        {
            // arrange
            var exception = GetException();

            // act
            var hasSocketsNamespace = HasSource(exception, "System.Net.Sockets");
            var hasSecurityNamespace = HasSource(exception, "System.Net.Security");

            // assert
            Assert.IsFalse(hasSocketsNamespace);
            Assert.IsFalse(hasSecurityNamespace);
        }
    }
}