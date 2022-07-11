<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text"/>
<xsl:template match="/"><xsl:text>Date/Time,UserName,ScreenName,Action Perfomrmed,User Host Address,Description</xsl:text>
<xsl:text>&#13;&#10;</xsl:text>
	<xsl:for-each select="NewDataSet/Table">
	<xsl:value-of select="DateTime"/>
	<xsl:text>&#44;</xsl:text>
	<xsl:value-of select="UserName"/>
	<xsl:text>&#44;</xsl:text>
	<xsl:value-of select="ScreenName"/>
	<xsl:text>&#44;</xsl:text>
	<xsl:value-of select="ActionPerformed"/>
	<xsl:text>&#44;</xsl:text> 
	<xsl:value-of select="UserHostAddress"/>
	<xsl:text>&#44;</xsl:text> 
	<xsl:value-of select="Description"/>
	<xsl:text>&#13;&#10;</xsl:text>
</xsl:for-each>
</xsl:template>
</xsl:stylesheet>
