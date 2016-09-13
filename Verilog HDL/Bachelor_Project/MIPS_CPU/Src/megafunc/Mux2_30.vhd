-- megafunction wizard: %LPM_MUX%
-- GENERATION: STANDARD
-- VERSION: WM1.0
-- MODULE: LPM_MUX 

-- ============================================================
-- File Name: Mux2_30.vhd
-- Megafunction Name(s):
-- 			LPM_MUX
--
-- Simulation Library Files(s):
-- 			lpm
-- ============================================================
-- ************************************************************
-- THIS IS A WIZARD-GENERATED FILE. DO NOT EDIT THIS FILE!
--
-- 11.0 Build 157 04/27/2011 SJ Full Version
-- ************************************************************


--Copyright (C) 1991-2011 Altera Corporation
--Your use of Altera Corporation's design tools, logic functions 
--and other software and tools, and its AMPP partner logic 
--functions, and any output files from any of the foregoing 
--(including device programming or simulation files), and any 
--associated documentation or information are expressly subject 
--to the terms and conditions of the Altera Program License 
--Subscription Agreement, Altera MegaCore Function License 
--Agreement, or other applicable license agreement, including, 
--without limitation, that your use is for the sole purpose of 
--programming logic devices manufactured by Altera and sold by 
--Altera or its authorized distributors.  Please refer to the 
--applicable agreement for further details.


LIBRARY ieee;
USE ieee.std_logic_1164.all;

LIBRARY lpm;
USE lpm.lpm_components.all;

ENTITY Mux2_30 IS
	PORT
	(
		data0x		: IN STD_LOGIC_VECTOR (29 DOWNTO 0);
		data1x		: IN STD_LOGIC_VECTOR (29 DOWNTO 0);
		sel		: IN STD_LOGIC ;
		result		: OUT STD_LOGIC_VECTOR (29 DOWNTO 0)
	);
END Mux2_30;


ARCHITECTURE SYN OF mux2_30 IS

--	type STD_LOGIC_2D is array (NATURAL RANGE <>, NATURAL RANGE <>) of STD_LOGIC;

	SIGNAL sub_wire0	: STD_LOGIC_VECTOR (29 DOWNTO 0);
	SIGNAL sub_wire1	: STD_LOGIC_VECTOR (29 DOWNTO 0);
	SIGNAL sub_wire2	: STD_LOGIC_2D (1 DOWNTO 0, 29 DOWNTO 0);
	SIGNAL sub_wire3	: STD_LOGIC_VECTOR (29 DOWNTO 0);
	SIGNAL sub_wire4	: STD_LOGIC ;
	SIGNAL sub_wire5	: STD_LOGIC_VECTOR (0 DOWNTO 0);

BEGIN
	sub_wire3    <= data0x(29 DOWNTO 0);
	result    <= sub_wire0(29 DOWNTO 0);
	sub_wire1    <= data1x(29 DOWNTO 0);
	sub_wire2(1, 0)    <= sub_wire1(0);
	sub_wire2(1, 1)    <= sub_wire1(1);
	sub_wire2(1, 2)    <= sub_wire1(2);
	sub_wire2(1, 3)    <= sub_wire1(3);
	sub_wire2(1, 4)    <= sub_wire1(4);
	sub_wire2(1, 5)    <= sub_wire1(5);
	sub_wire2(1, 6)    <= sub_wire1(6);
	sub_wire2(1, 7)    <= sub_wire1(7);
	sub_wire2(1, 8)    <= sub_wire1(8);
	sub_wire2(1, 9)    <= sub_wire1(9);
	sub_wire2(1, 10)    <= sub_wire1(10);
	sub_wire2(1, 11)    <= sub_wire1(11);
	sub_wire2(1, 12)    <= sub_wire1(12);
	sub_wire2(1, 13)    <= sub_wire1(13);
	sub_wire2(1, 14)    <= sub_wire1(14);
	sub_wire2(1, 15)    <= sub_wire1(15);
	sub_wire2(1, 16)    <= sub_wire1(16);
	sub_wire2(1, 17)    <= sub_wire1(17);
	sub_wire2(1, 18)    <= sub_wire1(18);
	sub_wire2(1, 19)    <= sub_wire1(19);
	sub_wire2(1, 20)    <= sub_wire1(20);
	sub_wire2(1, 21)    <= sub_wire1(21);
	sub_wire2(1, 22)    <= sub_wire1(22);
	sub_wire2(1, 23)    <= sub_wire1(23);
	sub_wire2(1, 24)    <= sub_wire1(24);
	sub_wire2(1, 25)    <= sub_wire1(25);
	sub_wire2(1, 26)    <= sub_wire1(26);
	sub_wire2(1, 27)    <= sub_wire1(27);
	sub_wire2(1, 28)    <= sub_wire1(28);
	sub_wire2(1, 29)    <= sub_wire1(29);
	sub_wire2(0, 0)    <= sub_wire3(0);
	sub_wire2(0, 1)    <= sub_wire3(1);
	sub_wire2(0, 2)    <= sub_wire3(2);
	sub_wire2(0, 3)    <= sub_wire3(3);
	sub_wire2(0, 4)    <= sub_wire3(4);
	sub_wire2(0, 5)    <= sub_wire3(5);
	sub_wire2(0, 6)    <= sub_wire3(6);
	sub_wire2(0, 7)    <= sub_wire3(7);
	sub_wire2(0, 8)    <= sub_wire3(8);
	sub_wire2(0, 9)    <= sub_wire3(9);
	sub_wire2(0, 10)    <= sub_wire3(10);
	sub_wire2(0, 11)    <= sub_wire3(11);
	sub_wire2(0, 12)    <= sub_wire3(12);
	sub_wire2(0, 13)    <= sub_wire3(13);
	sub_wire2(0, 14)    <= sub_wire3(14);
	sub_wire2(0, 15)    <= sub_wire3(15);
	sub_wire2(0, 16)    <= sub_wire3(16);
	sub_wire2(0, 17)    <= sub_wire3(17);
	sub_wire2(0, 18)    <= sub_wire3(18);
	sub_wire2(0, 19)    <= sub_wire3(19);
	sub_wire2(0, 20)    <= sub_wire3(20);
	sub_wire2(0, 21)    <= sub_wire3(21);
	sub_wire2(0, 22)    <= sub_wire3(22);
	sub_wire2(0, 23)    <= sub_wire3(23);
	sub_wire2(0, 24)    <= sub_wire3(24);
	sub_wire2(0, 25)    <= sub_wire3(25);
	sub_wire2(0, 26)    <= sub_wire3(26);
	sub_wire2(0, 27)    <= sub_wire3(27);
	sub_wire2(0, 28)    <= sub_wire3(28);
	sub_wire2(0, 29)    <= sub_wire3(29);
	sub_wire4    <= sel;
	sub_wire5(0)    <= sub_wire4;

	LPM_MUX_component : LPM_MUX
	GENERIC MAP (
		lpm_size => 2,
		lpm_type => "LPM_MUX",
		lpm_width => 30,
		lpm_widths => 1
	)
	PORT MAP (
		data => sub_wire2,
		sel => sub_wire5,
		result => sub_wire0
	);



END SYN;

-- ============================================================
-- CNX file retrieval info
-- ============================================================
-- Retrieval info: PRIVATE: INTENDED_DEVICE_FAMILY STRING "Cyclone III"
-- Retrieval info: PRIVATE: SYNTH_WRAPPER_GEN_POSTFIX STRING "0"
-- Retrieval info: PRIVATE: new_diagram STRING "1"
-- Retrieval info: LIBRARY: lpm lpm.lpm_components.all
-- Retrieval info: CONSTANT: LPM_SIZE NUMERIC "2"
-- Retrieval info: CONSTANT: LPM_TYPE STRING "LPM_MUX"
-- Retrieval info: CONSTANT: LPM_WIDTH NUMERIC "30"
-- Retrieval info: CONSTANT: LPM_WIDTHS NUMERIC "1"
-- Retrieval info: USED_PORT: data0x 0 0 30 0 INPUT NODEFVAL "data0x[29..0]"
-- Retrieval info: USED_PORT: data1x 0 0 30 0 INPUT NODEFVAL "data1x[29..0]"
-- Retrieval info: USED_PORT: result 0 0 30 0 OUTPUT NODEFVAL "result[29..0]"
-- Retrieval info: USED_PORT: sel 0 0 0 0 INPUT NODEFVAL "sel"
-- Retrieval info: CONNECT: @data 1 0 30 0 data0x 0 0 30 0
-- Retrieval info: CONNECT: @data 1 1 30 0 data1x 0 0 30 0
-- Retrieval info: CONNECT: @sel 0 0 1 0 sel 0 0 0 0
-- Retrieval info: CONNECT: result 0 0 30 0 @result 0 0 30 0
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux2_30.vhd TRUE
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux2_30.inc FALSE
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux2_30.cmp FALSE
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux2_30.bsf TRUE
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux2_30_inst.vhd FALSE
-- Retrieval info: LIB_FILE: lpm
