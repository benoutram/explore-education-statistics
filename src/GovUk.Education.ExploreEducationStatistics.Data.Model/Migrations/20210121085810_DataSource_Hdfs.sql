IF NOT EXISTS(SELECT * FROM sys.external_data_sources WHERE name = 'MyHadoopDs')
BEGIN
  CREATE EXTERNAL DATA SOURCE MyHadoopDs
  WITH
  (
     TYPE = HADOOP, -- Don't include with Azure BDC?
     -- Local Dev
     LOCATION = 'hdfs://sandbox-hdp.hortonworks.com:8020'
     -- Azure BDC
     -- LOCATION = 'sqlhdfs://controller-svc/default'
  );
END