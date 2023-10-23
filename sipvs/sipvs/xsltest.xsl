<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
							  xmlns:taxbonusform="http://www.taxbonusform.com">

  <!-- Match the root element of your XML -->
  <xsl:template match="taxbonusform:FormData">
    <html>
      <head>
        <title>Vyhlasenie</title>
		  <style>
    
          .rectangle {
            border: 1px solid #000;
            padding: 5px;
            margin: 5px;
            display: inline-block;
            width: 300px;
            height: 20px;

          }
          .label {
            font-weight: bold;
            display: block;
          }
		  .flex{
			display: flex;
		  }
          body{
            width: 50%;
          }

          .center{
            text-align: center;
          }
        </style>
      </head>
      <body>
        <!-- Apply styles to name and surname elements -->
        <div class="center">
            <h4>VYHLÁSENIE</h4>
            <div>
            na uplatnenie nezdaniteľnej časti základu dane na daňovníka a daňového bonusu podľa § 36 ods. 6
            neexistujúceho zákona o dani z príjmov v znení neskorších predpisov (ďalej len „vyhlásenie“)
            </div>
            <br/>
            <br/>
            <DIV>
                Vyhlásenie podľa § 36 ods. 6 neexistujúceho zákona o dani z príjmov v znení neskorších predpisov (ďalej len „zákon“) doručí zamestnanec zamestnávateľovi, ktorý je platiteľom dane (ďalej len „zamestnávateľ“), u ktorého si uplatňuje nárok na nezdaniteľnú časť základu dane na daňovníka a nárok na daňový bonus (§ 33). Ak má zamestnanec súčasne viacerých zamestnávateľov, vyhlásenie predloží len jednému z nich.
            </DIV>
        </div>

        <h5>I. ÚDAJE O ZAMESTNANCOVI</h5>
        <div class="flex">
            <div>
		        <p>Priezvisko </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:surname"/>
		        </div>
            </div>
            <div>
		        <p>Meno </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:name"/>
		        </div>
            </div>
            <div>
		        <p>Vek </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:age"/>
		        </div>
            </div>

        </div>
        <div class="flex">
            <div>
		        <p>Titul (pred menom) </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:degreeBefore"/>
		        </div>
            </div>
            <div>
		        <p>Titul (za menom) </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:degreeAfter"/>
		        </div>
            </div>
            <div>
		        <p>Rodinný stav </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:maritalStatus"/>
		        </div>
            </div>

        </div>
        <div class="flex">
            <div>
		        <p>Ulica </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:streetName"/>
		        </div>
            </div>
            <div>
		        <p>Súpisné/orientačné číslo </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:houseNum"/>
		        </div>
            </div>
            <div>
		        <p>PSČ </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:postCode"/>
		        </div>
            </div>

        </div>
        <div class="flex">
            <div>
		        <p>Obec </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:city"/>
		        </div>
            </div>
            <div>
		        <p>Štát </p>
		        <div class="rectangle">
		        	<xsl:value-of select="taxbonusform:country"/>
		        </div>
            </div>


        </div>
        
        
       <h5>II.ÚDAJE NA UPLATNENIE DAŇOVÉHO BONUSU PODĽA § 33 ZÁKONA</h5>
       <div>
        <input type="checkbox" disabled="true">
          <xsl:if test="taxbonusform:tax = 'true'">
            <xsl:attribute name="checked">checked</xsl:attribute>
          </xsl:if>
        </input>
        Uplatňujem si daňový bonus na dieťa (deti) žijúce so mnou v domácnosti
       </div>
          <xsl:for-each select="taxbonusform:kids/taxbonusform:Child">
           
            <div class="flex">
                <div>
		            <p>Meno a priezvisko </p>
		            <div class="rectangle">
		            	<xsl:value-of select="taxbonusform:fullname"/>
		            </div>
                </div>
                <div>
		            <p>Vek </p>
		            <div class="rectangle">
		            	<xsl:value-of select="taxbonusform:age"/>
		            </div>
                </div>


            </div>
        </xsl:for-each>

          <p>Dňa: <xsl:value-of select="taxbonusform:dateFormat/taxbonusform:date" /></p>
       
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
