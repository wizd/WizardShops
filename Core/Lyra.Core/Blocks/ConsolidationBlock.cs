﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyra.Core.Blocks
{
    [BsonIgnoreExtraElements]
    public class ConsolidationBlock : Block
    {
        public List<string> blockHashes { get; set; }
        public string MerkelTreeHash { get; set; }

        public override BlockTypes GetBlockType()
        {
            return BlockTypes.Consolidation;
        }

        protected override string GetExtraData()
        {
            string nui = string.Empty;
            if (blockHashes != null && blockHashes.Count > 0)
            {
                nui = blockHashes.Aggregate(new StringBuilder(),
                          (sb, h) => sb.Append($"|{h}"),
                          sb => sb.ToString());
            }
            return base.GetExtraData() +
                nui +
                $"|{MerkelTreeHash}";
        }
    }

    // Sync block:
    // 1) allows smooth transition between authorization samples; 
    // 2) ensures that stuck transactions are cleaned up.
    // Sync block does not include any changes; the samples is taken from the latest service block.
    // Transactions that reference an old service or sync block (< height - 1) are rejected.
    // The block is generated by the authorizer with largest stake;
    // If no block is generated within 2 minutes, the authorizer with the second stake must generate it, etc.
    //public class SyncBlock : Block
    //{
    //    public string LastServiceBlockHash { get; set; }

    //    public override BlockTypes GetBlockType()
    //    {
    //        return BlockTypes.Sync;
    //    }
    //}
}
