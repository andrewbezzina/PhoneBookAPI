CREATE TABLE Companies (
    CompanyId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name NVARCHAR(255) NOT NULL UNIQUE, 
    RegistrationDate DATE NOT NULL
);

GO

CREATE TABLE People (
    PersonId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	FullName NVARCHAR(255) NOT NULL,
	PhoneNumber NVARCHAR(20) NOT NULL,
	Address	NVARCHAR(255) NOT NULL,
	CompanyId INT NOT NULL,
    FOREIGN KEY (CompanyId) REFERENCES Companies(CompanyId)
);

GO 

CREATE INDEX People_Company_Key_Index
ON People (CompanyId);

GO
