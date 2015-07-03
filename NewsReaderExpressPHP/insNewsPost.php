<?php
	include"connect.php";
	include"Sanitize.php";
	
	$headLine = clean($con, $_POST['headLine']);
	$fileName = clean($con, $_POST['fileName']);
	$type = clean($con, $_POST['type']);
	$details = clean($con, $_POST['details']);
	$date = date('Y-m-d', strtotime("now"));
	
	if ($_FILES['image'] != null) 
	{
		$target = "Images/";
		$target = $target . basename( $_FILES['image']['name']);
		
		$imageUri = "http://localhost:21750/NewsReaderExpressPHP/" . $target;
		
		if(!(move_uploaded_file($_FILES['image']['tmp_name'], $target)))
		{
			//Tells you if its all ok
			die("Sorry, there was a problem uploading your file.");
		}
		
	}
    else
	{
		die("no image data found");
	}
	
	echo $fileName;
	
	$ins = mysqli_query($con, "INSERT INTO `newsposts`(`Heading`, `ImagePath`, `NewsType`, `Details`, `Date`) VALUES ('$headLine', '$imageUri', '$type', '$details', '$date')");
	if(!$ins)
	{
		die("Failed to insert data for work plan try re-inserting");
	}
	else 
	{
		die("Success");
	}
		    

	
?>