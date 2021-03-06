﻿using PhoneNumbers;
using Subscriber.DataContract;
using Subscriber.DataPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber.Service
{
    public class ContactsManager : IContactsManager
    {
        private IUserProfileRepository repo;
        public ContactsManager(IUserProfileRepository repo)
        {
            this.repo = repo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<RegisteredContact>> GetRegisteredContacts(Contacts request)
        {
            //this will be probalematic when the number of users grow
            List<PhoneContact> registeredList = await repo.GetRegisteredUsers();
            List<PhoneContact> phoneContactList = new List<PhoneContact>();

            request.ContactList.ForEach(phoneNumber => phoneContactList.Add(ParseNumber(phoneNumber, request.RequestorCountryCode)));

            return from pc in phoneContactList
                   join rc in registeredList
                   on new { pc.MobileNumber, pc.CountryCode } equals new { rc.MobileNumber, rc.CountryCode }
                   select new RegisteredContact() { UserId = rc.UserId, MobileNumberStoredInRequestorPhone = pc.MobileNumberStoredInRequestorPhone };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static PhoneContact ParseNumber(string number, string requestorCountryCode)
        {
            PhoneContact pc = new PhoneContact();
            pc.MobileNumberStoredInRequestorPhone = number;
            try
            {
                var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                int countryCode = phoneNumberUtil.MaybeExtractCountryCode(number, null, new StringBuilder(number), true, new PhoneNumber.Builder());
                string region = "IN";
                if (countryCode == 0)
                {
                    countryCode = int.Parse(requestorCountryCode.Split("+", StringSplitOptions.None)[1]);
                }
                region = phoneNumberUtil.GetRegionCodeForCountryCode(countryCode);
                var phoneNumber = phoneNumberUtil.Parse(number, region);

                pc.CountryCode = "+" + phoneNumber.CountryCode.ToString();
                pc.MobileNumber = phoneNumber.NationalNumber.ToString();

            }
            catch
            {
            }
            return pc;
        }

        public async Task<Dictionary<Guid, string>> GetGCMClientIds(IEnumerable<Guid> userIds)
        {
            return await repo.GetGCMClientIds(userIds);
        }
    }
}
