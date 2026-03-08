--Gustavo 31/05/2021 - Todo tiene VarChar, hay que modificar el SqlDbType

select 'par[' + ltrim(STR(b.colorder-1)) + '] = new SqlParameter();' + char(13) + 
'par[' + ltrim(STR(b.colorder-1)) + '].ParameterName = "@' + ltrim(rtrim(b.name)) + '";' + char(13) +  
'par[' + ltrim(STR(b.colorder-1)) + '].Value = "_' + ltrim(rtrim(b.name)) + '";'  + char(13) + 
'par[' + ltrim(STR(b.colorder-1)) + '].SqlDbType = SqlDbType.VarChar;' + CHAR(10) + char(13) 
from sysobjects a inner join syscolumns b on a.id = b.id and a.name = 'VENTClientes' order by b.colorder


/*
par[0] = new SqlParameter();
            par[0].ParameterName = "@sSucursalID";
            par[0].Value = _sClienteID;
            par[0].SqlDbType = SqlDbType.VarChar;.


			*/