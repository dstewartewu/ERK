<?php
	ob_start();
	
	//import all the necessary stuff from the library
	require_once('./Barcode/class/BCGFontFile.php');
	require_once('./Barcode/class/BCGColor.php');
	require_once('./Barcode/class/BCGDrawing.php');
	require_once('./Barcode/class/BCGcode39.barcode.php');
	
	//Get font for barcode text
	$font = new BCGFontFile('./Barcode/font/Arial.ttf', 18);
	//Get colors for barcode
	$color_black = new BCGColor(0, 0, 0);
	$color_white = new BCGColor(255, 255, 255);
	//Get color for font background
	$colorFont = new BCGColor(255,0,0);
	
	// Barcode Part
	$code = new BCGcode39();
	$code->setScale(2);
	$code->setThickness(30);
	$code->setForegroundColor($color_black);
	$code->setBackgroundColor($color_white);
	$code->setFont($font);
	$code->parse($postCode);
	 
	// Drawing Part
	$drawing = new BCGDrawing('', $colorFont);
	$drawing->setBarcode($code);
	$drawing->draw();
	
	$drawing->finish(BCGDrawing::IMG_FORMAT_PNG);

	//put barcode into variable
	$img = ob_get_contents();
	ob_end_clean();
	//return barcode
	return $img;
?>