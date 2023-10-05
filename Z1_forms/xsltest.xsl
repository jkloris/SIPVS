<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- Match the root element of your XML -->
  <xsl:template match="FormData">
    <html>
      <head>
        <title>Styled XML</title>
        <style>
          /* Define styles for the rectangles */
          .rectangle {
            border: 1px solid #000;
            padding: 5px;
            margin: 5px;
            display: inline-block;
          }

          /* Style for labels */
          .label {
            font-weight: bold;
            display: block;
          }
        </style>
      </head>
      <body>
        <!-- Apply styles to each XML element -->
        <xsl:apply-templates select="*"/>
      </body>
    </html>
  </xsl:template>

  <!-- Template for XML elements -->
  <xsl:template match="*">
    <div class="rectangle">
      <span class="label">
        <xsl:value-of select="name()"/>:
      </span>
      <xsl:choose>
        <xsl:when test="not(node())">
          <i>Empty</i>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="."/>
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>

</xsl:stylesheet>
