--Create a vehicle table
CREATE TABLE vehicle (
    vehicle_id int identity (1,1) primary key,
    vehicle_year varchar(50) NOT NULL,
    vehicle_make varchar(255) not null,
    vehicle_model varchar (50) not null,
	vehicle_number char (20),
	vehicle_mileage char (50),
	vehicle_plate char (20),
	vehicle_no_default char (20),
);
--Users table
CREATE TABLE users(
	users_last_name varchar(50) NOT NULL,
	users_first_name varchar(50) NOT NULL,
	users_telephone char(10) NULL,
	users_email varchar(50) NULL,
	users_password char(20) NULL,
	users_DL char(20) NULL,
	users_DL_state [char](20) NULL,
	users_type char(20) NULL,
	vehicle_id int NULL,
	user_id int IDENTITY(1,1) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
--Inspection table
CREATE TABLE inspection(
	inspection_id int IDENTITY(1,1) NOT NULL,
	inspection_beginning_mileage decimal(18, 0) NOT NULL,
	inspection_ending_mileage decimal(18, 0) NOT NULL,
	inspection_total_mileage_driven decimal(18, 0) NULL,
	inspection_last_oil_change_date date NULL,
	inspection_oil_change_due date NULL,
	inspection_interval varchar(10) NULL,
	inspection_last_tire_rotation date NULL,
	inspection_tires_rotation_due date NULL,
	inspection_tires_pressure decimal(18, 0) NULL,
	vehicle_number int NULL,
	inspection_additional_notes varchar(250) NULL,
	trip_fluid_level varchar(10) NULL,
	battery_good varchar(10) NULL,
	gauge_working varchar(10) NULL,
	clean_cab varchar(10) NULL,
	clean_exterior varchar(10) NULL,
	inspection_date date NULL,
PRIMARY KEY CLUSTERED 
(
	[inspection_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--Trip table
CREATE TABLE trip(
	trip_date date NULL,
	trip_beginning_mileage char(20) NOT NULL,
	trip_destination varchar(100) NOT NULL,
	trip_purpose varchar(100) NOT NULL,
	trip_ending_mileage varchar(20) NOT NULL,
	trip_total_miles char(20) NULL,
	vehicle_id varchar(150) NULL,
	user_id int NULL
);

--**Reports-----
---Inspection Report--------

/****** Object:  StoredProcedure [dbo].[inspection_report]    Script Date: 7/11/2023 9:58:03 PM ******/
/****** Object:  StoredProcedure [dbo].[inspection_report]    Script Date: 7/12/2023 10:57:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure inspection_report
(@beginning_date date, @ending_date date, @selected_vehicle int=null) as
if @selected_vehicle is null and @beginning_date is not null and @ending_date  is not null
begin
select 
cast (v.vehicle_year+ ' '+v.vehicle_make+ ' ' +v.vehicle_model+ ' ' +v.vehicle_plate as varchar(50)) 'Vehicle',
i.inspection_beginning_mileage 'Beginning Mileage', i.inspection_ending_mileage 'Ending Mileage',
CONVERT(VARCHAR(10),i.inspection_last_oil_change_date,101) 'Last Oil Change', CONVERT(VARCHAR(10), i.inspection_oil_change_due,101) 'Oil Change Due', 
CONVERT(VARCHAR(10),i.inspection_last_tire_rotation,101) 'Last Tire Change', CONVERT(VARCHAR(10),i.inspection_tires_rotation_due,101) 'Rotation Due',
i.inspection_tires_pressure 'Tire Pressure', i.inspection_additional_notes 'Additional Notes'
from inspection i join vehicle v on i.vehicle_number=v.vehicle_id
where inspection_date between @beginning_date and @ending_date
order by i.inspection_oil_change_due, inspection_date asc
end

else
begin
select 
cast (v.vehicle_year+ ' '+v.vehicle_make+ ' ' +v.vehicle_model+ ' ' +v.vehicle_plate as varchar(50)) 'Vehicle',
i.inspection_beginning_mileage 'Beginning Mileage', i.inspection_ending_mileage 'Ending Mileage',
CONVERT(VARCHAR(10),i.inspection_last_oil_change_date,101) 'Last Oil Change',CONVERT(VARCHAR(10), i.inspection_oil_change_due,101) 'Oil Change Due', 
CONVERT(VARCHAR(10),i.inspection_last_tire_rotation,101) 'Last Tire Change', CONVERT(VARCHAR(10),i.inspection_tires_rotation_due,101) 'Rotation Due',
i.inspection_tires_pressure 'Tire Pressure', i.inspection_additional_notes 'Additional Notes'
from inspection i join vehicle v on i.vehicle_number=v.vehicle_id
where inspection_date between @beginning_date and @ending_date

and @selected_vehicle=v.vehicle_id
order by i.inspection_oil_change_due, inspection_date asc
end
GO

--Trip Report
/****** Object:  StoredProcedure [dbo].[trip_report_new]    Script Date: 7/12/2023 10:58:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Trip Date, Trip Destination, Trip Purpose, Trip Total Miles

--ALTER TABLE table_name
--RENAME COLUMN old_name TO new_name;

--ALTER TABLE employee ALTER COLUMN emp_parking_space BIGINT;


--EXEC sp_rename 'trip_report_new.trip_date', 'Trip Date';

CREATE procedure trip_report_new
(@trip_beginning_date DATETIME, @trip_ending_date DATETIME, @selectd_vehicle INT = null, @select_user INT = null) AS
if @select_user is null and @selectd_vehicle is null			-- SCENARIO 1: Date parameters included but No User and no Vehicle parameter
	begin
		print '1'
		select cast(u.users_last_name+', '+u.users_first_name as char(20)) 'Full Name', CONVERT(VARCHAR(10),t.trip_date,101) 'Trip Date', t.trip_destination 'Trip Destination',
		t.trip_purpose 'Trip Purpose', t.trip_total_miles 'Trip Total Miles', cast (v.vehicle_year+ ' '+v.vehicle_make+ ' ' +v.vehicle_model+ ' ' +v.vehicle_plate as varchar(50)) 'Vehicle'
		from trip t join vehicle v on t.vehicle_id=v.vehicle_id join users u on (t.user_id = u.user_id)
		where trip_date between @trip_beginning_date and  @trip_ending_date
		order by trip_date asc
	end
else if @select_user is null and @selectd_vehicle is NOT null	-- SCENARIO 2: Date and Vehicle parameters included but No User parameter
	begin
		print '2'
		select cast(u.users_last_name+', '+u.users_first_name as char(20)) 'Full Name',CONVERT(VARCHAR(10), t.trip_date,101) 'Trip Date', t.trip_destination 'Trip Destination',
		t.trip_purpose 'Trip Purpose', t.trip_total_miles'Trip Total Miles', cast (v.vehicle_year+ ' '+v.vehicle_make+ ' ' +v.vehicle_model+ ' ' +v.vehicle_plate as varchar(50)) 'Vehicle'
		from trip t join vehicle v on t.vehicle_id=v.vehicle_id join users u on (t.user_id = u.user_id)
		where trip_date between @trip_beginning_date and  @trip_ending_date and @selectd_vehicle = t.vehicle_id
		order by trip_date asc
	end
else if @select_user is not null and @selectd_vehicle is null	--  SCENARIO 3: Date and User parameters included but No Vehicle
	begin
		print '3'
		select cast(u.users_last_name+', '+u.users_first_name as char(20)) 'Full Name',CONVERT(VARCHAR(10), t.trip_date,101) 'Trip Date', t.trip_destination 'Trip Destination',
		t.trip_purpose 'Trip Purpose', t.trip_total_miles 'Trip Total Miles', cast (v.vehicle_year+ ' '+v.vehicle_make+ ' ' +v.vehicle_model+ ' ' +v.vehicle_plate as varchar(50)) 'Vehicle'
		from trip t join vehicle v on t.vehicle_id=v.vehicle_id join users u on (t.user_id = u.user_id)
		where trip_date between @trip_beginning_date and  @trip_ending_date and @select_user = u.user_id
		order by trip_date asc
	end
	else   -- SCENARIO 4: Both User and Vehicle parameters included with Date parameters
		begin
		print '4'
		select cast(u.users_last_name+', '+u.users_first_name as char(20)) 'Full Name',CONVERT(VARCHAR(10), t.trip_date,101) 'Trip Date', t.trip_destination 'Trip Destination',
		t.trip_purpose 'Trip Purpose', t.trip_total_miles 'Trip Total Miles', cast (v.vehicle_year+ ' '+v.vehicle_make+ ' ' +v.vehicle_model+ ' ' +v.vehicle_plate as varchar(50)) 'Vehicle'
		from trip t join vehicle v on t.vehicle_id=v.vehicle_id join users u on (t.user_id = u.user_id)
		where @select_user = u.user_id and @selectd_vehicle = t.vehicle_id and trip_date between @trip_beginning_date and  @trip_ending_date
		order by trip_date asc
end

GO