using Alachisoft.NCache.Runtime.Dependencies;
using System;
using System.Collections.Generic;

namespace NHibernate.Caches.NCache
{
    public class SQLDependencyConfig : CommandDependencyConfig
    {
        public override DatabaseDependencyType DatabaseDependencyType =>
            DatabaseDependencyType.Sql;

        public override CacheDependency GetCacheDependency(
            string connectionString,
            object key)
        {
            var cmdType =
                    IsStoredProcedure ?
                        SqlCommandType.StoredProcedure :
                        SqlCommandType.Text;

            string cmdText = GetCommand();

            var cmdParams = new Dictionary<string, SqlCmdParams>();

            if (!string.IsNullOrWhiteSpace(PrimaryKeyInputParameter))
            {
                if (!ValidateKey(key))
                {
                    if (key == null)
                    {
                        throw new ArgumentException($"Key cannot be null");
                    }
                    throw new ArgumentOutOfRangeException(
                        $"Key type {key.GetType().FullName} not supported");
                }

                var cmdParam = new SqlCmdParams();
                cmdParam.Value = key;
                cmdParam.Type = GetCommandParamType(PrimaryKeyDbType);
                cmdParam.Direction = SqlParamDirection.Input;

                var cmdParamKey = IsStoredProcedure ?
                                    $"{PrimaryKeyInputParameter}" :
                                    $"@{PrimaryKeyInputParameter}";

                cmdParams.Add(cmdParamKey, cmdParam);
            }

            if (IsStoredProcedure)
            {
                foreach (var pair in OutputParametersAndDbTypes)
                {
                    var pairKey = pair.Key;
                    var pairType = pair.Value;

                    if (string.IsNullOrWhiteSpace(pairKey) ||
                        string.IsNullOrWhiteSpace(pairType))
                    {
                        throw new ArgumentNullException(
                            "Key/value pair invalid. Please check your " +
                            "dependency configuration");
                    }
                    var cmdParam = new SqlCmdParams();
                    cmdParam.Type = GetCommandParamType(pairType);
                    cmdParam.Direction = SqlParamDirection.Output;
                    cmdParam.Size = int.MaxValue;

                    cmdParams.Add(pairKey, cmdParam);
                }
            }

            return new SqlCacheDependency(
                    connectionString,
                    cmdText,
                    cmdType,
                    cmdParams);
        }

        private string GetCommand()
        {
            return GetCommand('@');
        }

        private CmdParamsType GetCommandParamType(string propertyType)
        {
            if (propertyType.Equals(
                                "bigint",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.BigInt;
            }
            else if (propertyType.Equals(
                                "binary",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Binary;
            }
            else if (propertyType.Equals(
                                "bit",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Bit;
            }
            else if (propertyType.Equals(
                                "char",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Char;
            }
            else if (propertyType.Equals(
                                "date",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Date;
            }
            else if (propertyType.Equals(
                                "datetime",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.DateTime;
            }
            else if (propertyType.Equals(
                                "datetime2",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.DateTime2;
            }
            else if (propertyType.Equals(
                                "datetimeoffset",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.DateTimeOffset;
            }
            else if (propertyType.Equals(
                                "decimal",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Decimal;
            }
            else if (propertyType.Equals(
                                "float",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Float;
            }
            else if (propertyType.Equals(
                                "int",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Int;
            }
            else if (propertyType.Equals(
                                "money",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Money;
            }
            else if (propertyType.Equals(
                                "nchar",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.NChar;
            }
            else if (propertyType.Equals(
                                "nvarchar",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.NVarChar;
            }
            else if (propertyType.Equals(
                                "real",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Real;
            }
            else if (propertyType.Equals(
                                "smalldatetime",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.SmallDateTime;
            }
            else if (propertyType.Equals(
                                "smallint",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.SmallInt;
            }
            else if (propertyType.Equals(
                                "smallmoney",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.SmallMoney;
            }
            else if (propertyType.Equals("structured"))
            {
                return CmdParamsType.Structured;
            }
            else if (propertyType.Equals(
                                "time",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Time;
            }
            else if (propertyType.Equals(
                                "timestamp",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Timestamp;
            }
            else if (propertyType.Equals(
                                "tinyint",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.TinyInt;
            }
            else if (propertyType.Equals(
                                "udt",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Udt;
            }
            else if (propertyType.Equals(
                                "uniqueidentifier",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.UniqueIdentifier;
            }
            else if (propertyType.Equals(
                                "varbinary",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.VarBinary;
            }
            else if (propertyType.Equals(
                                "varchar",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.VarChar;
            }
            else if (propertyType.Equals(
                                "variant",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Variant;
            }
            else if (propertyType.Equals(
                                "xml",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return CmdParamsType.Xml;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    $"SQL db type not supported",
                    nameof(propertyType));
            }
        }
    }
}
