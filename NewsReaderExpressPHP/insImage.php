<?php 
	
	if ($_FILES['image'] != null) 
	{
		$target = "Images/";
		$target = $target . basename( $_FILES['image']['name']);
		
		//if($_FILES['image'] != null) die($target . "File exists");
		if($_FILES['image'] == null) die("File does not exists");

		if(move_uploaded_file($_FILES['image']['tmp_name'], $target))
		{
			//Tells you if its all ok
			die("The file has been uploaded, and your information has been added to the directory");
		}
		else {

			//Gives and error if its not
			die("Sorry, there was a problem uploading your file.");
		}
	}
    else
	{
		die("no image data found");
	}		
?>