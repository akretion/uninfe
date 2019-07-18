:: Enquanto a gente não colocar no nuget, manter este carinha aqui,
:: pois ele copia todo o conteúdo pro ERP.Net.
:: Estamos fazendo muitas alterações.

SET destDir=Z:\erp.net\trunk\fontes\Compilacao\Unimake.DFe
MD %destDir%
     
COPY Z:\uninfe\trunk\fontes\Unimake.DFe\Compilacao\netstandard2.0\Unimake.Business.DFe.dll %destDir%\Unimake.Business.DFe.dll /Y  
COPY Z:\uninfe\trunk\fontes\Unimake.DFe\Compilacao\netstandard2.0\Unimake.Security.Platform.dll %destDir%\Unimake.Security.Platform.dll /Y  
COPY Z:\uninfe\trunk\fontes\Unimake.DFe\Compilacao\netstandard2.0\Unimake.Business.DFe.pdb %destDir%\Unimake.Business.DFe.pdb /Y  
COPY Z:\uninfe\trunk\fontes\Unimake.DFe\Compilacao\netstandard2.0\Unimake.Security.Platform.pdb %destDir%\Unimake.Security.Platform.pdb /Y  

EXIT 0