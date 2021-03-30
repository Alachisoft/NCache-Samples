using Alachisoft.NCache.Runtime.Dependencies;
using System;
using System.Collections.Generic;

namespace NHibernate.Caches.NCache
{
    public class OracleDependencyConfig : CommandDependencyConfig
    {
        public override DatabaseDependencyType DatabaseDependencyType =>
            DatabaseDependencyType.Oracle;

        public override CacheDependency GetCacheDependency(
            string connectionString,
            object key)
        {
            var cmdType = IsStoredProcedure ?
                            OracleCommandType.StoredProcedure :
                            OracleCommandType.Text;

            string cmdText = GetCommand();

            var cmdParams = new Dictionary<string, OracleCmdParams>();

            if (!string.IsNullOrWhiteSpace(PrimaryKeyInputParameter))
            {
                if (!ValidateKey(key))
                {
                    if (key == null)
                    {
                        throw new ArgumentException(
                            $"Key cannot be null");
                    }
                    throw new ArgumentOutOfRangeException(
                        $"Key type {key.GetType().FullName} not supported");
                }

                var cmdParam = new OracleCmdParams();
                cmdParam.Value = key;
                cmdParam.Type = GetCommandParamType(PrimaryKeyDbType);
                cmdParam.Direction = OracleParameterDirection.Input;

                cmdParams.Add(PrimaryKeyInputParameter, cmdParam);
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
                            "Key/value pair invalid with empty or null values. " +
                            "Please check your dependency configuration");
                    }
                    var cmdParam = new OracleCmdParams();
                    cmdParam.Type = GetCommandParamType(pairType);
                    cmdParam.Direction = OracleParameterDirection.Output;

                    var cmdParamKey = $"{pairKey}";

                    cmdParams.Add(cmdParamKey, cmdParam);
                }
            }

            return new OracleCacheDependency(
                    connectionString,
                    cmdText,
                    cmdType,
                    cmdParams);
        }

        private OracleCmdParamsType GetCommandParamType(string propertyType)
        {
            if (propertyType.Equals(
                                "bfile",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.BFile;
            }
            else if (propertyType.Equals(
                                "blob",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Blob;
            }
            else if (propertyType.Equals(
                                "byte",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Byte;
            }
            else if (propertyType.Equals(
                                "char",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Char;
            }
            else if (propertyType.Equals(
                                "clob",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Clob;
            }
            else if (propertyType.Equals(
                                "date",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Date;
            }
            else if (propertyType.Equals(
                                "decimal",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Decimal;
            }
            else if (propertyType.Equals(
                                "double",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Double;
            }
            else if (propertyType.Equals(
                                "int16",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Int16;
            }
            else if (propertyType.Equals(
                                "int32",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Int32;
            }
            else if (propertyType.Equals(
                                "int64",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Int64;
            }
            else if (propertyType.Equals(
                                "intervalids",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.IntervalDS;
            }
            else if (propertyType.Equals(
                                "intervalym",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.IntervalYM;
            }
            else if (propertyType.Equals(
                                "long",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Long;
            }
            else if (propertyType.Equals(
                                "longraw",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.LongRaw;
            }
            else if (propertyType.Equals(
                                "nchar",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.NChar;
            }
            else if (propertyType.Equals(
                                "nclob",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.NClob;
            }
            else if (propertyType.Equals(
                                "nvarchar2",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.NVarchar2;
            }
            else if (propertyType.Equals(
                                "raw",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Raw;
            }
            else if (propertyType.Equals(
                                "refcursor",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.RefCursor;
            }
            else if (propertyType.Equals(
                                "single",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Single;
            }
            else if (propertyType.Equals(
                                "timestamp",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.TimeStamp;
            }
            else if (propertyType.Equals(
                                "timestampltz",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.TimeStampLTZ;
            }
            else if (propertyType.Equals(
                                "timestamptz",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.TimeStampTZ;
            }
            else if (propertyType.Equals(
                                "varchar2",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.Varchar2;
            }
            else if (propertyType.Equals(
                                "xmltype",
                                StringComparison.InvariantCultureIgnoreCase))
            {
                return OracleCmdParamsType.XmlType;
            }
            else
            {
                throw new ArgumentException(
                    $"Oracle db type not supported",
                    nameof(propertyType));
            }
        }

        private string GetCommand()
        {
            return GetCommand(':');
        }
    }
}
