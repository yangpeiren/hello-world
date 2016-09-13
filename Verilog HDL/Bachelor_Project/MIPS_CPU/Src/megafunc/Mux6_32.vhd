-- megafunction wizard: %LPM_MUX%
-- GENERATION: STANDARD
-- VERSION: WM1.0
-- MODULE: LPM_MUX 

-- ============================================================
-- File Name: Mux6_32.vhd
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

ENTITY Mux6_32 IS
	PORT
	(
		data0x		: IN STD_LOGIC_VECTOR (31 DOWNTO 0);
		data1x		: IN STD_LOGIC_VECTOR (31 DOWNTO 0);
		data2x		: IN STD_LOGIC_VECTOR (31 DOWNTO 0);
		data3x		: IN STD_LOGIC_VECTOR (31 DOWNTO 0);
		data4x		: IN STD_LOGIC_VECTOR (31 DOWNTO 0);
		data5x		: IN STD_LOGIC_VECTOR (31 DOWNTO 0);
		sel		: IN STD_LOGIC_VECTOR (2 DOWNTO 0);
		result		: OUT STD_LOGIC_VECTOR (31 DOWNTO 0)
	);
END Mux6_32;


ARCHITECTURE SYN OF mux6_32 IS

--	type STD_LOGIC_2D is array (NATURAL RANGE <>, NATURAL RANGE <>) of STD_LOGIC;

	SIGNAL sub_wire0	: STD_LOGIC_VECTOR (31 DOWNTO 0);
	SIGNAL sub_wire1	: STD_LOGIC_VECTOR (31 DOWNTO 0);
	SIGNAL sub_wire2	: STD_LOGIC_2D (5 DOWNTO 0, 31 DOWNTO 0);
	SIGNAL sub_wire3	: STD_LOGIC_VECTOR (31 DOWNTO 0);
	SIGNAL sub_wire4	: STD_LOGIC_VECTOR (31 DOWNTO 0);
	SIGNAL sub_wire5	: STD_LOGIC_VECTOR (31 DOWNTO 0);
	SIGNAL sub_wire6	: STD_LOGIC_VECTOR (31 DOWNTO 0);
	SIGNAL sub_wire7	: STD_LOGIC_VECTOR (31 DOWNTO 0);

BEGIN
	sub_wire7    <= data0x(31 DOWNTO 0);
	sub_wire6    <= data1x(31 DOWNTO 0);
	sub_wire5    <= data2x(31 DOWNTO 0);
	sub_wire4    <= data3x(31 DOWNTO 0);
	sub_wire3    <= data4x(31 DOWNTO 0);
	result    <= sub_wire0(31 DOWNTO 0);
	sub_wire1    <= data5x(31 DOWNTO 0);
	sub_wire2(5, 0)    <= sub_wire1(0);
	sub_wire2(5, 1)    <= sub_wire1(1);
	sub_wire2(5, 2)    <= sub_wire1(2);
	sub_wire2(5, 3)    <= sub_wire1(3);
	sub_wire2(5, 4)    <= sub_wire1(4);
	sub_wire2(5, 5)    <= sub_wire1(5);
	sub_wire2(5, 6)    <= sub_wire1(6);
	sub_wire2(5, 7)    <= sub_wire1(7);
	sub_wire2(5, 8)    <= sub_wire1(8);
	sub_wire2(5, 9)    <= sub_wire1(9);
	sub_wire2(5, 10)    <= sub_wire1(10);
	sub_wire2(5, 11)    <= sub_wire1(11);
	sub_wire2(5, 12)    <= sub_wire1(12);
	sub_wire2(5, 13)    <= sub_wire1(13);
	sub_wire2(5, 14)    <= sub_wire1(14);
	sub_wire2(5, 15)    <= sub_wire1(15);
	sub_wire2(5, 16)    <= sub_wire1(16);
	sub_wire2(5, 17)    <= sub_wire1(17);
	sub_wire2(5, 18)    <= sub_wire1(18);
	sub_wire2(5, 19)    <= sub_wire1(19);
	sub_wire2(5, 20)    <= sub_wire1(20);
	sub_wire2(5, 21)    <= sub_wire1(21);
	sub_wire2(5, 22)    <= sub_wire1(22);
	sub_wire2(5, 23)    <= sub_wire1(23);
	sub_wire2(5, 24)    <= sub_wire1(24);
	sub_wire2(5, 25)    <= sub_wire1(25);
	sub_wire2(5, 26)    <= sub_wire1(26);
	sub_wire2(5, 27)    <= sub_wire1(27);
	sub_wire2(5, 28)    <= sub_wire1(28);
	sub_wire2(5, 29)    <= sub_wire1(29);
	sub_wire2(5, 30)    <= sub_wire1(30);
	sub_wire2(5, 31)    <= sub_wire1(31);
	sub_wire2(4, 0)    <= sub_wire3(0);
	sub_wire2(4, 1)    <= sub_wire3(1);
	sub_wire2(4, 2)    <= sub_wire3(2);
	sub_wire2(4, 3)    <= sub_wire3(3);
	sub_wire2(4, 4)    <= sub_wire3(4);
	sub_wire2(4, 5)    <= sub_wire3(5);
	sub_wire2(4, 6)    <= sub_wire3(6);
	sub_wire2(4, 7)    <= sub_wire3(7);
	sub_wire2(4, 8)    <= sub_wire3(8);
	sub_wire2(4, 9)    <= sub_wire3(9);
	sub_wire2(4, 10)    <= sub_wire3(10);
	sub_wire2(4, 11)    <= sub_wire3(11);
	sub_wire2(4, 12)    <= sub_wire3(12);
	sub_wire2(4, 13)    <= sub_wire3(13);
	sub_wire2(4, 14)    <= sub_wire3(14);
	sub_wire2(4, 15)    <= sub_wire3(15);
	sub_wire2(4, 16)    <= sub_wire3(16);
	sub_wire2(4, 17)    <= sub_wire3(17);
	sub_wire2(4, 18)    <= sub_wire3(18);
	sub_wire2(4, 19)    <= sub_wire3(19);
	sub_wire2(4, 20)    <= sub_wire3(20);
	sub_wire2(4, 21)    <= sub_wire3(21);
	sub_wire2(4, 22)    <= sub_wire3(22);
	sub_wire2(4, 23)    <= sub_wire3(23);
	sub_wire2(4, 24)    <= sub_wire3(24);
	sub_wire2(4, 25)    <= sub_wire3(25);
	sub_wire2(4, 26)    <= sub_wire3(26);
	sub_wire2(4, 27)    <= sub_wire3(27);
	sub_wire2(4, 28)    <= sub_wire3(28);
	sub_wire2(4, 29)    <= sub_wire3(29);
	sub_wire2(4, 30)    <= sub_wire3(30);
	sub_wire2(4, 31)    <= sub_wire3(31);
	sub_wire2(3, 0)    <= sub_wire4(0);
	sub_wire2(3, 1)    <= sub_wire4(1);
	sub_wire2(3, 2)    <= sub_wire4(2);
	sub_wire2(3, 3)    <= sub_wire4(3);
	sub_wire2(3, 4)    <= sub_wire4(4);
	sub_wire2(3, 5)    <= sub_wire4(5);
	sub_wire2(3, 6)    <= sub_wire4(6);
	sub_wire2(3, 7)    <= sub_wire4(7);
	sub_wire2(3, 8)    <= sub_wire4(8);
	sub_wire2(3, 9)    <= sub_wire4(9);
	sub_wire2(3, 10)    <= sub_wire4(10);
	sub_wire2(3, 11)    <= sub_wire4(11);
	sub_wire2(3, 12)    <= sub_wire4(12);
	sub_wire2(3, 13)    <= sub_wire4(13);
	sub_wire2(3, 14)    <= sub_wire4(14);
	sub_wire2(3, 15)    <= sub_wire4(15);
	sub_wire2(3, 16)    <= sub_wire4(16);
	sub_wire2(3, 17)    <= sub_wire4(17);
	sub_wire2(3, 18)    <= sub_wire4(18);
	sub_wire2(3, 19)    <= sub_wire4(19);
	sub_wire2(3, 20)    <= sub_wire4(20);
	sub_wire2(3, 21)    <= sub_wire4(21);
	sub_wire2(3, 22)    <= sub_wire4(22);
	sub_wire2(3, 23)    <= sub_wire4(23);
	sub_wire2(3, 24)    <= sub_wire4(24);
	sub_wire2(3, 25)    <= sub_wire4(25);
	sub_wire2(3, 26)    <= sub_wire4(26);
	sub_wire2(3, 27)    <= sub_wire4(27);
	sub_wire2(3, 28)    <= sub_wire4(28);
	sub_wire2(3, 29)    <= sub_wire4(29);
	sub_wire2(3, 30)    <= sub_wire4(30);
	sub_wire2(3, 31)    <= sub_wire4(31);
	sub_wire2(2, 0)    <= sub_wire5(0);
	sub_wire2(2, 1)    <= sub_wire5(1);
	sub_wire2(2, 2)    <= sub_wire5(2);
	sub_wire2(2, 3)    <= sub_wire5(3);
	sub_wire2(2, 4)    <= sub_wire5(4);
	sub_wire2(2, 5)    <= sub_wire5(5);
	sub_wire2(2, 6)    <= sub_wire5(6);
	sub_wire2(2, 7)    <= sub_wire5(7);
	sub_wire2(2, 8)    <= sub_wire5(8);
	sub_wire2(2, 9)    <= sub_wire5(9);
	sub_wire2(2, 10)    <= sub_wire5(10);
	sub_wire2(2, 11)    <= sub_wire5(11);
	sub_wire2(2, 12)    <= sub_wire5(12);
	sub_wire2(2, 13)    <= sub_wire5(13);
	sub_wire2(2, 14)    <= sub_wire5(14);
	sub_wire2(2, 15)    <= sub_wire5(15);
	sub_wire2(2, 16)    <= sub_wire5(16);
	sub_wire2(2, 17)    <= sub_wire5(17);
	sub_wire2(2, 18)    <= sub_wire5(18);
	sub_wire2(2, 19)    <= sub_wire5(19);
	sub_wire2(2, 20)    <= sub_wire5(20);
	sub_wire2(2, 21)    <= sub_wire5(21);
	sub_wire2(2, 22)    <= sub_wire5(22);
	sub_wire2(2, 23)    <= sub_wire5(23);
	sub_wire2(2, 24)    <= sub_wire5(24);
	sub_wire2(2, 25)    <= sub_wire5(25);
	sub_wire2(2, 26)    <= sub_wire5(26);
	sub_wire2(2, 27)    <= sub_wire5(27);
	sub_wire2(2, 28)    <= sub_wire5(28);
	sub_wire2(2, 29)    <= sub_wire5(29);
	sub_wire2(2, 30)    <= sub_wire5(30);
	sub_wire2(2, 31)    <= sub_wire5(31);
	sub_wire2(1, 0)    <= sub_wire6(0);
	sub_wire2(1, 1)    <= sub_wire6(1);
	sub_wire2(1, 2)    <= sub_wire6(2);
	sub_wire2(1, 3)    <= sub_wire6(3);
	sub_wire2(1, 4)    <= sub_wire6(4);
	sub_wire2(1, 5)    <= sub_wire6(5);
	sub_wire2(1, 6)    <= sub_wire6(6);
	sub_wire2(1, 7)    <= sub_wire6(7);
	sub_wire2(1, 8)    <= sub_wire6(8);
	sub_wire2(1, 9)    <= sub_wire6(9);
	sub_wire2(1, 10)    <= sub_wire6(10);
	sub_wire2(1, 11)    <= sub_wire6(11);
	sub_wire2(1, 12)    <= sub_wire6(12);
	sub_wire2(1, 13)    <= sub_wire6(13);
	sub_wire2(1, 14)    <= sub_wire6(14);
	sub_wire2(1, 15)    <= sub_wire6(15);
	sub_wire2(1, 16)    <= sub_wire6(16);
	sub_wire2(1, 17)    <= sub_wire6(17);
	sub_wire2(1, 18)    <= sub_wire6(18);
	sub_wire2(1, 19)    <= sub_wire6(19);
	sub_wire2(1, 20)    <= sub_wire6(20);
	sub_wire2(1, 21)    <= sub_wire6(21);
	sub_wire2(1, 22)    <= sub_wire6(22);
	sub_wire2(1, 23)    <= sub_wire6(23);
	sub_wire2(1, 24)    <= sub_wire6(24);
	sub_wire2(1, 25)    <= sub_wire6(25);
	sub_wire2(1, 26)    <= sub_wire6(26);
	sub_wire2(1, 27)    <= sub_wire6(27);
	sub_wire2(1, 28)    <= sub_wire6(28);
	sub_wire2(1, 29)    <= sub_wire6(29);
	sub_wire2(1, 30)    <= sub_wire6(30);
	sub_wire2(1, 31)    <= sub_wire6(31);
	sub_wire2(0, 0)    <= sub_wire7(0);
	sub_wire2(0, 1)    <= sub_wire7(1);
	sub_wire2(0, 2)    <= sub_wire7(2);
	sub_wire2(0, 3)    <= sub_wire7(3);
	sub_wire2(0, 4)    <= sub_wire7(4);
	sub_wire2(0, 5)    <= sub_wire7(5);
	sub_wire2(0, 6)    <= sub_wire7(6);
	sub_wire2(0, 7)    <= sub_wire7(7);
	sub_wire2(0, 8)    <= sub_wire7(8);
	sub_wire2(0, 9)    <= sub_wire7(9);
	sub_wire2(0, 10)    <= sub_wire7(10);
	sub_wire2(0, 11)    <= sub_wire7(11);
	sub_wire2(0, 12)    <= sub_wire7(12);
	sub_wire2(0, 13)    <= sub_wire7(13);
	sub_wire2(0, 14)    <= sub_wire7(14);
	sub_wire2(0, 15)    <= sub_wire7(15);
	sub_wire2(0, 16)    <= sub_wire7(16);
	sub_wire2(0, 17)    <= sub_wire7(17);
	sub_wire2(0, 18)    <= sub_wire7(18);
	sub_wire2(0, 19)    <= sub_wire7(19);
	sub_wire2(0, 20)    <= sub_wire7(20);
	sub_wire2(0, 21)    <= sub_wire7(21);
	sub_wire2(0, 22)    <= sub_wire7(22);
	sub_wire2(0, 23)    <= sub_wire7(23);
	sub_wire2(0, 24)    <= sub_wire7(24);
	sub_wire2(0, 25)    <= sub_wire7(25);
	sub_wire2(0, 26)    <= sub_wire7(26);
	sub_wire2(0, 27)    <= sub_wire7(27);
	sub_wire2(0, 28)    <= sub_wire7(28);
	sub_wire2(0, 29)    <= sub_wire7(29);
	sub_wire2(0, 30)    <= sub_wire7(30);
	sub_wire2(0, 31)    <= sub_wire7(31);

	LPM_MUX_component : LPM_MUX
	GENERIC MAP (
		lpm_size => 6,
		lpm_type => "LPM_MUX",
		lpm_width => 32,
		lpm_widths => 3
	)
	PORT MAP (
		data => sub_wire2,
		sel => sel,
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
-- Retrieval info: CONSTANT: LPM_SIZE NUMERIC "6"
-- Retrieval info: CONSTANT: LPM_TYPE STRING "LPM_MUX"
-- Retrieval info: CONSTANT: LPM_WIDTH NUMERIC "32"
-- Retrieval info: CONSTANT: LPM_WIDTHS NUMERIC "3"
-- Retrieval info: USED_PORT: data0x 0 0 32 0 INPUT NODEFVAL "data0x[31..0]"
-- Retrieval info: USED_PORT: data1x 0 0 32 0 INPUT NODEFVAL "data1x[31..0]"
-- Retrieval info: USED_PORT: data2x 0 0 32 0 INPUT NODEFVAL "data2x[31..0]"
-- Retrieval info: USED_PORT: data3x 0 0 32 0 INPUT NODEFVAL "data3x[31..0]"
-- Retrieval info: USED_PORT: data4x 0 0 32 0 INPUT NODEFVAL "data4x[31..0]"
-- Retrieval info: USED_PORT: data5x 0 0 32 0 INPUT NODEFVAL "data5x[31..0]"
-- Retrieval info: USED_PORT: result 0 0 32 0 OUTPUT NODEFVAL "result[31..0]"
-- Retrieval info: USED_PORT: sel 0 0 3 0 INPUT NODEFVAL "sel[2..0]"
-- Retrieval info: CONNECT: @data 1 0 32 0 data0x 0 0 32 0
-- Retrieval info: CONNECT: @data 1 1 32 0 data1x 0 0 32 0
-- Retrieval info: CONNECT: @data 1 2 32 0 data2x 0 0 32 0
-- Retrieval info: CONNECT: @data 1 3 32 0 data3x 0 0 32 0
-- Retrieval info: CONNECT: @data 1 4 32 0 data4x 0 0 32 0
-- Retrieval info: CONNECT: @data 1 5 32 0 data5x 0 0 32 0
-- Retrieval info: CONNECT: @sel 0 0 3 0 sel 0 0 3 0
-- Retrieval info: CONNECT: result 0 0 32 0 @result 0 0 32 0
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux6_32.vhd TRUE
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux6_32.inc FALSE
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux6_32.cmp FALSE
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux6_32.bsf TRUE
-- Retrieval info: GEN_FILE: TYPE_NORMAL Mux6_32_inst.vhd FALSE
-- Retrieval info: LIB_FILE: lpm
