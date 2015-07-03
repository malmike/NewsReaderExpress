<?php
	function clean($con, $str)
	{
		$str = @trim($str);
		
		if(get_magic_quotes_gpc())
		{
			$str = stripslashes($str);
		}
		return mysqli_real_escape_string($con, $str);	
	}
	
	function normalClean($str)
	{
		$str = @trim($str);
		
		if(get_magic_quotes_gpc())
		{
			$str = stripslashes($str);
		}
		return $str;
	}
?>