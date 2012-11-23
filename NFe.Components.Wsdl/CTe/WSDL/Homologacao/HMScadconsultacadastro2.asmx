<?xml version="1.0" encoding="utf-8"?>
<wsdl:definitions xmlns:s="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://schemas.xmlsoap.org/wsdl/soap12/" xmlns:mime="http://schemas.xmlsoap.org/wsdl/mime/" xmlns:tns="http://www.portalfiscal.inf.br/cte/wsdl/CadConsultaCadastro" xmlns:soap="http://schemas.xmlsoap.org/wsdl/soap/" xmlns:tm="http://microsoft.com/wsdl/mime/textMatching/" xmlns:http="http://schemas.xmlsoap.org/wsdl/http/" xmlns:soapenc="http://schemas.xmlsoap.org/soap/encoding/" targetNamespace="http://www.portalfiscal.inf.br/cte/wsdl/CadConsultaCadastro" xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">
  <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Serviço para consultar o cadastro de contribuintes do ICMS da unidade federada.</wsdl:documentation>
  <wsdl:types>
    <s:schema elementFormDefault="qualified" targetNamespace="http://www.portalfiscal.inf.br/cte/wsdl/CadConsultaCadastro">
      <s:element name="cteDadosMsg">
        <s:complexType mixed="true">
          <s:sequence>
            <s:any />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="consultaCadastroResult">
        <s:complexType mixed="true">
          <s:sequence>
            <s:any />
          </s:sequence>
        </s:complexType>
      </s:element>
      <s:element name="cteCabecMsg" type="tns:CTeCabecMsg" />
      <s:complexType name="CTeCabecMsg">
        <s:sequence>
          <s:element minOccurs="0" maxOccurs="1" name="cUF" type="s:string" />
          <s:element minOccurs="0" maxOccurs="1" name="versaoDados" type="s:string" />
        </s:sequence>
        <s:anyAttribute />
      </s:complexType>
    </s:schema>
  </wsdl:types>
  <wsdl:message name="consultaCadastroSoap12In">
    <wsdl:part name="cteDadosMsg" element="tns:cteDadosMsg" />
  </wsdl:message>
  <wsdl:message name="consultaCadastroSoap12Out">
    <wsdl:part name="consultaCadastroResult" element="tns:consultaCadastroResult" />
  </wsdl:message>
  <wsdl:message name="consultaCadastrocteCabecMsg">
    <wsdl:part name="cteCabecMsg" element="tns:cteCabecMsg" />
  </wsdl:message>
  <wsdl:portType name="CadConsultaCadastroSoap12">
    <wsdl:operation name="consultaCadastro">
      <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Consulta Cadastro</wsdl:documentation>
      <wsdl:input message="tns:consultaCadastroSoap12In" />
      <wsdl:output message="tns:consultaCadastroSoap12Out" />
    </wsdl:operation>
  </wsdl:portType>
  <wsdl:binding name="CadConsultaCadastroSoap12" type="tns:CadConsultaCadastroSoap12">
    <soap12:binding transport="http://schemas.xmlsoap.org/soap/http" />
    <wsdl:operation name="consultaCadastro">
      <soap12:operation soapAction="http://www.portalfiscal.inf.br/cte/wsdl/CadConsultaCadastro/consultaCadastro" style="document" />
      <wsdl:input>
        <soap12:body use="literal" />
        <soap12:header message="tns:consultaCadastrocteCabecMsg" part="cteCabecMsg" use="literal" />
      </wsdl:input>
      <wsdl:output>
        <soap12:body use="literal" />
        <soap12:header message="tns:consultaCadastrocteCabecMsg" part="cteCabecMsg" use="literal" />
      </wsdl:output>
    </wsdl:operation>
  </wsdl:binding>
  <wsdl:service name="CadConsultaCadastro">
    <wsdl:documentation xmlns:wsdl="http://schemas.xmlsoap.org/wsdl/">Serviço para consultar o cadastro de contribuintes do ICMS da unidade federada.</wsdl:documentation>
    <wsdl:port name="CadConsultaCadastroSoap12" binding="tns:CadConsultaCadastroSoap12">
      <soap12:address location="https://producao.cte.ms.gov.br/cteWEB/CadConsultaCadastro.asmx" />
    </wsdl:port>
  </wsdl:service>
</wsdl:definitions>