Module modEnum

    Enum enSendError
        lngErrNumber
        strErrSource
        strErrDescription
        intNotificationType
        strAppTitle
        strAppVersion
        lngErrLine
        blnCustomError
        strApplicationID
        intSaveErrorFileInterval
        strErrorFilePrefix
        strErrorFilePath
        strAdditionalInfo
        strMailFrom
        strMailTo
        strMailCC
        strMailSubject
        strUserHostAddress
        strActionPerformed
        strAlarmTagName
        enMax
    End Enum

    Enum UserType
        ReadOnlyUser = 1
        TeamLeader = 2
        Supervisor = 3
        Schedulers = 7
        Administrator = 44
    End Enum


    Enum psHoldType
        OnHold
        OffHold
    End Enum

    Enum psNodeType
        SHIFT = 1
        LOT = 2
        SUBLOT = 3
    End Enum

    Enum StationStyleGroupType
        StyleGroupID
        StyleGroupName
    End Enum

    Enum StationsType
        StationID
        Description
    End Enum

    Enum UserAccountType
        UserID
        FirstName
        LastName
        LogInName
        Password
        UserTypeID
        ModifiedDT
        ChangedByUser
        StationBypassEnabled
        BarcodeIdentifier
        BadgeID
        LoginLevel
        PLCUser
    End Enum
End Module